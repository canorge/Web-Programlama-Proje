﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;



namespace WebDeneme2.Controllers
{
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
                TumHizmetler = tumHizmetler
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CalisanViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Çalışanı id'ye göre veritabanından bul
                var calisan = await _dataContext.Calisanlar
                                                 .Include(c => c.UzmanlikAlanlari)
                                                 .ThenInclude(u => u.Hizmet)
                                                 .FirstOrDefaultAsync(c => c.Id == model.Calisan.Id);
                var calisanHizmet = await _dataContext.CalisanHizmetleri.FirstOrDefaultAsync(ch => ch.Calisan.Id == calisan.Id);
                if (calisan == null)
                {
                    return NotFound();  // Çalışan bulunamadıysa 404 döner
                }

                // Çalışan bilgilerini güncelle
                calisan.Ad = model.Calisan.Ad;
                calisan.Soyad = model.Calisan.Soyad;
                calisan.Email = model.Calisan.Email;
                calisan.TelNo = model.Calisan.TelNo;

                // Uzmanlık alanlarını temizle ve yeni verileri ekle
                calisan.UzmanlikAlanlari.Clear();
                foreach (var hizmetıd in model.HizmetIds)  // hizmetıds formdan gönderilen seçili hizmetlerin id'leri
                {
                    calisan.UzmanlikAlanlari.Add(new CalisanHizmet
                    {
                        HizmetId = hizmetıd,
                        CalisanId = calisan.Id
                    });
                }

                
                foreach(var hizmet in model.HizmetIds)
                {
                    calisanHizmet.HizmetId = hizmet;
                }
                _dataContext.CalisanHizmetleri.Update(calisanHizmet);
                // Çalışan bilgilerini güncelle ve veritabanına kaydet
                _dataContext.Calisanlar.Update(calisan);
                await _dataContext.SaveChangesAsync();

                // Başarıyla güncellendikten sonra tekrar çalışanın detay sayfasına yönlendir
                return RedirectToAction("Index", new { id = calisan.Id });
            }

            // Model geçersizse aynı sayfayı tekrar göster
            return RedirectToAction("Index", new { id = model.Calisan.Id });
        }




        public async Task<IActionResult> Randevular(int id)
        {
            var calisan=await _dataContext.Calisanlar
                                                     .Include(m=> m.Randevular)
                                                     .ThenInclude(m=>m.Musteri)
                                                     .FirstOrDefaultAsync(m=>m.Id==id);
            if(calisan == null)
            {
                return NotFound();
            }
            return View(calisan);   
        }

    }
}

         