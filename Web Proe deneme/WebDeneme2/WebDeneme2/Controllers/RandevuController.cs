using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    //[Authorize] // Tüm action'lar için yetkilendirme kontrolü
    public class RandevuController : Controller
    {
        private readonly DataContext _context;

        public RandevuController(DataContext context)
        {
            _context = context;
        }

        // Randevu oluşturma formunu gösteren action
        public async Task<IActionResult> Create()
        {
            var role = HttpContext.Session.GetString("Role");

            // Eğer oturumda müşteri rolü yoksa Giriş Yap sayfasına yönlendir
            if (string.IsNullOrEmpty(role) || role != "Müşteri")
            {
                return RedirectToAction("Index", "GirisYap");
            }
            ViewBag.Hizmetler = await _context.Hizmetler.ToListAsync();
            return View();
        }

        // Randevu oluşturma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu,int id)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı kimliğini al
                //var userEmail = User.Identity?.Name;

               
                // Müşteriyi veritabanından bul
                var musteri = await _context.Musteriler
                    .FirstOrDefaultAsync(m => m.Id==id);
                if(musteri == null)
                {
                    return View();
                }
                //if (musteri == null )
                //{
                //    return Unauthorized();
                //}

                // Randevuya müşteri bilgisi ekle
                randevu.MusteriId = musteri.Id;

                // Çalışanın uygunluğunu kontrol et
                var hizmet = await _context.Hizmetler.FindAsync(randevu.HizmetId);
                if (hizmet == null)
                {
                    ModelState.AddModelError("", "Seçilen hizmet bulunamadı.");
                    return View(randevu);
                }

                var randevuBaslangic = randevu.RandevuTarihi;
                var randevuBitis = randevu.RandevuTarihi.AddMinutes(hizmet.Sure);

                bool calisanRandevuVar = await _context.Randevular
                    .Include(r => r.Hizmet)
                    .Where(r => r.CalisanId == randevu.CalisanId)
                    .AnyAsync(r =>
                        (r.RandevuTarihi < randevuBitis &&
                         r.RandevuTarihi.AddMinutes(r.Hizmet.Sure) > randevuBaslangic));

                if (calisanRandevuVar)
                {
                    ModelState.AddModelError("", "Seçilen çalışan bu tarih ve saat aralığında dolu.");
                    return View(randevu);
                }

                _context.Randevular.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MusteriPanel");
            }

            return View(randevu);
        }

        // Belirli bir hizmeti veren çalışanları döndüren API
        [HttpGet]
        public async Task<IActionResult> GetCalisanlarByHizmetId(int hizmetId)
        {
            var calisanlar = await _context.CalisanHizmetleri
                .Where(ch => ch.HizmetId == hizmetId)
                .Select(ch => new
                {
                    id = ch.Calisan.Id,
                    AdSoyad = !string.IsNullOrEmpty(ch.Calisan.Ad) && !string.IsNullOrEmpty(ch.Calisan.Soyad)
                      ? $"{ch.Calisan.Ad} {ch.Calisan.Soyad}"
                      : "Ad/Soyad Bilgisi Yok"
                })
                .ToListAsync();

            return Json(calisanlar);
        }
    }
}
