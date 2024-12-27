using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Filters;
using WebDeneme2.Models;



namespace WebDeneme2.Controllers
{
    [CustomAuthorize("Calisan")]
    public class CalisanPanelController :Controller
    {
        private readonly DataContext _dataContext;

        public CalisanPanelController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(int Id)
        {
            var calisan = await _dataContext.Calisanlar
                                     .Include(c => c.UzmanlikAlanlari)
                                     .ThenInclude(u => u.Hizmet)
                                     .FirstOrDefaultAsync(c => c.Id == Id);

            if (calisan == null)
            {
                return NotFound();
            }

            // Tüm hizmetleri getir
            var tumHizmetler = await _dataContext.Hizmetler.ToListAsync();
          

            var model = new CalisanViewModel
            {
                Calisan = calisan,
                TumHizmetler = tumHizmetler,
                HizmetIds = calisan.UzmanlikAlanlari.Select(uzmanlik => uzmanlik.HizmetId).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CalisanViewModel model)
        {
            if(model == null)
            {
                return NotFound();
            }
            if (model.TumHizmetler == null)
            {
                model.TumHizmetler = await _dataContext.Hizmetler.ToListAsync();
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                // Geçerli olmayan model durumunda yapılacak işlemler
                return View(model);
            }

            // Çalışanı veritabanından al
            var calisan = await _dataContext.Calisanlar.Include(c => c.UzmanlikAlanlari)
                                                        .FirstOrDefaultAsync(c => c.Id == model.Calisan.Id);

            if (calisan == null)
            {
                return NotFound(); // Eğer çalışan bulunamazsa 404 döner
            }

            // Çalışan bilgilerini güncelle
            calisan.Ad = model.Calisan.Ad;
            calisan.Soyad = model.Calisan.Soyad;
            calisan.Email = model.Calisan.Email;
            calisan.TelNo = model.Calisan.TelNo;

            _dataContext.CalisanHizmetleri.RemoveRange(calisan.UzmanlikAlanlari);
            calisan.UzmanlikAlanlari.Clear();

            // Yeni uzmanlık alanlarını bul
            var yeniUzmanliklar = new List<CalisanHizmet>();
            foreach (var hizmetId in model.HizmetIds)
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
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Randevular(int id)
        {
            var calisan = await _dataContext.Calisanlar
                                     .Include(c => c.UzmanlikAlanlari)
                                     .ThenInclude(u => u.Hizmet)
                                     .Include(r=>r.Randevular.Where(t=>t.RandevuTarihi >= DateTime.Now))
                                     .ThenInclude(m=>m.Musteri)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (calisan == null)
            {
                return NotFound();
            }

            // Tüm hizmetleri getir
            var tumHizmetler = await _dataContext.Hizmetler.ToListAsync();

            var model = new CalisanViewModel
            {
                Calisan = calisan,
                TumHizmetler = tumHizmetler
            };

            return View(model);
        }

    }
}

         
