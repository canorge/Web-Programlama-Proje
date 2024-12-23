using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;
using WebDeneme2.Filters;


namespace WebDeneme2.Controllers
{
    [CustomAuthorize("Musteri")]
    public class MusteriPanelController :Controller
    {
        private readonly DataContext _dataContext;

        public MusteriPanelController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(int Id)
        {
            var musteri=await _dataContext.Musteriler.FirstOrDefaultAsync(m=>m.Id == Id);
            var model = new MusteriViewModel
            {
                Musteri = musteri
            };
            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> Index(MusteriViewModel model)
        {
            if (ModelState.IsValid)
            {
                var musteri = await _dataContext.Musteriler.FirstOrDefaultAsync(m=>m.Id==model.Musteri.Id);
                if (musteri == null )
                {
                    return NotFound();
                }
                    musteri.Ad = model.Musteri.Ad;
                    musteri.Soyad = model.Musteri.Soyad;
                    musteri.Email = model.Musteri.Email;
                    musteri.TelNo = model.Musteri.TelNo;

                    _dataContext.Musteriler.Update(musteri);
                    _dataContext.SaveChanges();
                    return RedirectToAction("Index", new { id = musteri.Id });
            }
            return RedirectToAction("Index", new { id = model.Musteri.Id });
        }

        public async Task<IActionResult> Randevular(int id)
        {
            var musteri=await _dataContext.Musteriler
                                                     .Include(m=> m.Randevular)
                                                     .ThenInclude(r=>r.Calisan)
                                                     .Include(m=>m.Randevular)
                                                     .ThenInclude(r=>r.Hizmet)
                                                     .FirstOrDefaultAsync(m=>m.Id==id);
            
            if(musteri == null)
            {
                return NotFound();
            }
            var model = new MusteriViewModel
            {
                Musteri = musteri
            };
            return View(model);   
        }

        public async Task<IActionResult> Create(int id)
        {
            var musteri= await _dataContext.Musteriler.FirstOrDefaultAsync(m=>m.Id==id);
            ViewBag.hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new MusteriViewModel
            {
                Musteri = musteri
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MusteriViewModel model)//int id,
        {
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();
            if (ModelState.IsValid)
            {
                var musteri = await _dataContext.Musteriler.FirstOrDefaultAsync(m => m.Id == model.Musteri.Id);
                if (musteri == null)
                {
                      
                    ViewBag.hizmetler = hizmetler;
                    return View(model);
                }
                
                var hizmet = await _dataContext.Hizmetler.FirstOrDefaultAsync(h=>h.Id==model.Randevu.HizmetId);
                if (hizmet == null)
                {
                    ModelState.AddModelError("", "Seçilen hizmet bulunamadı.");
                      
                    ViewBag.hizmetler = hizmetler;
                    return View(model);
                }

                var RandevuBaslangic = model.Randevu.RandevuTarihi;
                var BitisTarihi = model.Randevu.RandevuTarihi.AddMinutes(hizmet.Sure);

                bool MusaitMi= await _dataContext.Randevular
                    .Include(r => r.Hizmet)
                    .Where(r => r.CalisanId == model.Randevu.CalisanId && model.Randevu.RandevuDurum=="Onay")
                    .AnyAsync(r =>
                        (r.RandevuTarihi <= BitisTarihi &&
                         r.RandevuTarihi.AddMinutes(r.Hizmet.Sure) >= RandevuBaslangic));


                if (MusaitMi)
                {
                    ModelState.AddModelError("", "Seçilen çalışan bu tarih ve saat aralığında dolu.");
                    
                    ViewBag.hizmetler = hizmetler;
                    return View(model);
                }
                model.Randevu.RandevuDurum = "Bekliyor";
                model.Randevu.MusteriId = musteri.Id;
                _dataContext.Randevular.Add(model.Randevu);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index", new { id = model.Musteri.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            
            ViewBag.hizmetler = hizmetler;
            return View(model);
        }
        // Belirli bir hizmeti veren çalışanları döndüren API
        [HttpGet]
        public async Task<IActionResult> GetCalisanlarByHizmetId(int hizmetId)
        {
            var calisanlar = await _dataContext.CalisanHizmetleri
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

         
