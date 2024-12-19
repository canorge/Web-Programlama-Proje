using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    [Authorize] // Tüm action'lar için yetkilendirme kontrolü
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
            ViewBag.Hizmetler = new SelectList(await _context.Hizmetler.ToListAsync(), "Id", "Ad");
            return View();
        }

        // Randevu oluşturma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı kimliğini al
                var userEmail = User.Identity?.Name;

                if (userEmail == null)
                {
                    return RedirectToAction("Index", "GirisYap"); // Giriş sayfasına yönlendir
                }

                // Müşteriyi veritabanından bul
                var musteri = await _context.Musteriler
                    .FirstOrDefaultAsync(m => m.Email == userEmail);

                if (musteri == null)
                {
                    return Unauthorized();
                }

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
                    Id = ch.Calisan.Id,
                    AdSoyad = $"{ch.Calisan.Ad} {ch.Calisan.Soyad}"
                })
                .ToListAsync();

            return Json(calisanlar);
        }
    }
}
