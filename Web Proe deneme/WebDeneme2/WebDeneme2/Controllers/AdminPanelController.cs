﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;



namespace WebDeneme2.Controllers
{
    public class AdminPanelController :Controller
    {
        private readonly DataContext _dataContext;

        public AdminPanelController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(int id)
        {
            var admin=await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();
            var calisanlar = await _dataContext.Calisanlar .Include(c=>c.UzmanlikAlanlari)
                                                           .ThenInclude(ua=>ua.Hizmet)
                                                           .ToListAsync();
            var musteriler = await _dataContext.Musteriler.ToListAsync();
            var randevular = await _dataContext.Randevular.ToListAsync();
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new AdminViewModel
            {
                Admin = admin,
                Calisanlar = calisanlar,
                Musteriler = musteriler,
                Randevular = randevular,
                TumHizmetler = hizmetler
            };

            if (model.Admin == null)
            {
                return NotFound();
            }
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(Calisan model, List<int> HizmetIds)
        {
            if (!ModelState.IsValid)
            {
                // Geçerli olmayan model durumunda yapılacak işlemler
                return View(model);
            }

            // Çalışanı veritabanından al
            var calisan = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                        .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (calisan == null)
            {
                return NotFound(); // Eğer çalışan bulunamazsa 404 döner
            }

            // Çalışan bilgilerini güncelle
            calisan.Ad = model.Ad;
            calisan.Soyad = model.Soyad;
            calisan.Email = model.Email;
            calisan.TelNo = model.TelNo;

            _dataContext.CalisanHizmetleri.RemoveRange(calisan.UzmanlikAlanlari);
            calisan.UzmanlikAlanlari.Clear();

            // Yeni uzmanlık alanlarını bul
            var yeniUzmanliklar = new List<CalisanHizmet>();
            foreach (var hizmetId in HizmetIds)
            {
                // Bu hizmet daha önce eklenmemişse, ekle
                if (!calisan.UzmanlikAlanlari.Any(uzmanlik => uzmanlik.HizmetId == hizmetId))
                {
                    var yeniUzmanlik = new CalisanHizmet
                    {
                        HizmetId = hizmetId,
                        CalisanId = calisan.Id
                    };
                    calisan.UzmanlikAlanlari.Add(yeniUzmanlik);  // Calisan'ın UzmanlikAlanlari koleksiyonuna ekle
                    yeniUzmanliklar.Add(yeniUzmanlik);  // Yeni uzmanlıkları geçici listeye ekle
                }
            }

            if (yeniUzmanliklar.Any())
            {
                await _dataContext.CalisanHizmetleri.AddRangeAsync(yeniUzmanliklar);  // Yeni ilişkileri CalisanHizmet tablosuna ekle
            }

            // Veritabanında yapılan değişiklikleri kaydet
            _dataContext.Calisanlar.Update(calisan);
            await _dataContext.SaveChangesAsync();

            // Güncelleme başarılıysa, tekrar çalışanın listesine yönlendir
            return RedirectToAction("Index", "AdminPanel");
        }






        public async Task<IActionResult> Musteriler(int id)
        {
            var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();
            var calisanlar = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                           .ThenInclude(ua => ua.Hizmet)
                                                           .ToListAsync();
            var musteriler = await _dataContext.Musteriler.ToListAsync();
            var randevular = await _dataContext.Randevular.ToListAsync();
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new AdminViewModel
            {
                Admin = admin,
                Calisanlar = calisanlar,
                Musteriler = musteriler,
                Randevular = randevular,
                TumHizmetler = hizmetler
            };

            if (model.Admin == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Musteriler(Musteri model, int Id)
        {
            if (!ModelState.IsValid)
            {
                // Eğer model geçerli değilse, mevcut admin ve diğer verilere tekrar ihtiyacınız olabilir
                var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == Id);
                if (admin == null) return NotFound();

                var calisanlar = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                               .ThenInclude(ua => ua.Hizmet)
                                                               .ToListAsync();
                var musteriler = await _dataContext.Musteriler.ToListAsync();
                var randevular = await _dataContext.Randevular.ToListAsync();
                var hizmetler = await _dataContext.Hizmetler.ToListAsync();

                var viewModel = new AdminViewModel
                {
                    Admin = admin,
                    Calisanlar = calisanlar,
                    Musteriler = musteriler,
                    Randevular = randevular,
                    TumHizmetler = hizmetler
                };

                // View'e tekrar model döndür
                return View(viewModel);
            }

            // Müşteriyi veritabanından bul
            var musteri = await _dataContext.Musteriler.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (musteri == null)
            {
                return NotFound();
            }
            if(model == null)
            {
                return BadRequest("model boş");
            }

            // Bilgileri güncelle
            musteri.Ad = model.Ad;
            musteri.Soyad = model.Soyad;
            musteri.Email = model.Email;
            musteri.TelNo = model.TelNo;

            // Güncellemeyi kaydet
            _dataContext.Musteriler.Update(musteri);
            await _dataContext.SaveChangesAsync();

            // Başarıyla güncellendi, Musteriler sayfasına yönlendirme
            return RedirectToAction("Musteriler", "AdminPanel",new {id=Id} );
        }

        public async Task<IActionResult> Randevular(int id)
        {
            var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();
            var calisanlar = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                           .ThenInclude(ua => ua.Hizmet)
                                                           .ToListAsync();
            var musteriler = await _dataContext.Musteriler.ToListAsync();
            var randevular = await _dataContext.Randevular
                                                            .Where(r => r.RandevuDurum == "Onay") // Filter pending appointments
                                                            .Include(r => r.Hizmet)
                                                            .Include(r => r.Musteri)
                                                            .ToListAsync();
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new AdminViewModel
            {
                Admin = admin,
                Calisanlar = calisanlar,
                Musteriler = musteriler,
                Randevular = randevular,
                TumHizmetler = hizmetler
            };

            if (model.Admin == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> BekleyenRandevular(int id)
        {
            var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();
            var calisanlar = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                           .ThenInclude(ua => ua.Hizmet)
                                                           .ToListAsync();
            var musteriler = await _dataContext.Musteriler.ToListAsync();
            var randevular = await _dataContext.Randevular
                                                            .Where(r => r.RandevuDurum == "Bekliyor") // Filter pending appointments
                                                            .Include(r => r.Hizmet)
                                                            .Include(r => r.Musteri)
                                                            .ToListAsync();
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new AdminViewModel
            {
                Admin = admin,
                Calisanlar = calisanlar,
                Musteriler = musteriler,
                Randevular = randevular,
                TumHizmetler = hizmetler
            };

            if (model.Admin == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> RedRandevu(int id)
        {
            var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();
            var calisanlar = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                           .ThenInclude(ua => ua.Hizmet)
                                                           .ToListAsync();
            var musteriler = await _dataContext.Musteriler.ToListAsync();
            var randevular = await _dataContext.Randevular
                                                            .Where(r => r.RandevuDurum == "Red") // Filter pending appointments
                                                            .Include(r => r.Hizmet)
                                                            .Include(r => r.Musteri)
                                                            .ToListAsync();
            var hizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new AdminViewModel
            {
                Admin = admin,
                Calisanlar = calisanlar,
                Musteriler = musteriler,
                Randevular = randevular,
                TumHizmetler = hizmetler
            };

            if (model.Admin == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> TumRandevular(int id)
        {
            // Admin bilgilerini al
            var admin = await _dataContext.Adminler.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null) return NotFound();

            // Tüm randevuları al
            var tumRandevular = await _dataContext.Randevular
                                                  .Include(r => r.Hizmet) // Hizmet bilgilerini ekle
                                                  .Include(r => r.Musteri) // Müşteri bilgilerini ekle
                                                  .ToListAsync();

            // Model oluştur
            var model = new AdminViewModel
            {
                Admin = admin,
                Randevular = tumRandevular // Tüm randevuları ekle
            };

            return View(model);
        }


    }
}

         
