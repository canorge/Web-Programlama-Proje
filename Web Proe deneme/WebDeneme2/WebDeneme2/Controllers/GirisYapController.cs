using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class GirisYapController:Controller
    {
        private readonly DataContext _dataContext;
        public GirisYapController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Index(GirisViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var Musteri= await _dataContext.Musteriler.FirstOrDefaultAsync(m=>m.Email == model.Email && m.Sifre==model.Sifre);
            if (Musteri != null)
            {
                return RedirectToAction("Index","MusteriPanel", Musteri);
            }
            var Calisan = await _dataContext.Calisanlar.FirstOrDefaultAsync(c => c.Email == model.Email && c.Sifre == model.Sifre);
            if (Calisan != null)
            {
                //Calisan paneline
            }
            var Admin = await _dataContext.Calisanlar.FirstOrDefaultAsync(a => a.Email == model.Email && a.Sifre == model.Sifre);
            if (Calisan != null)
            {
                //admin paneline
            }
            return NotFound();
        }
        public IActionResult Update()
        {
            
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Update(SifreYenileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model doğrulaması geçmezse aynı sayfayı göster
            }

            var Musteri = await _dataContext.Musteriler.FirstOrDefaultAsync(m => m.Email == model.Email);

            if (Musteri != null)
            {
                Musteri.Sifre = model.Sifre;
                _dataContext.Musteriler.Update(Musteri);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }

            var Calisan = await _dataContext.Calisanlar.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (Calisan != null)
            {
                Calisan.Sifre = model.Sifre;
                _dataContext.Calisanlar.Update(Calisan);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }

            var Admin = await _dataContext.Adminler.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (Admin != null)
            {
                Admin.Sifre = model.Sifre;
                _dataContext.Adminler.Update(Admin);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async  Task<IActionResult> Create(MusteriRegister model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // LINQ Sorgusu
            var existingEmails = await _dataContext.Musteriler
                          .Select(m => m.Email)
                          .Union(_dataContext.Calisanlar.Select(c => c.Email))
                          .Union(_dataContext.Adminler.Select(a => a.Email))
                          .ToListAsync();

            if (existingEmails.Contains(model.Email))
            {
                ModelState.AddModelError("Email", "Bu email zaten kullanılıyor.");
                return View(model);
            }

            // LINQ Sorgusu
            var existingTelno = await _dataContext.Musteriler
                          .Select(m => m.TelNo)
                          .Union(_dataContext.Calisanlar.Select(c => c.TelNo))
                          .Union(_dataContext.Adminler.Select(a => a.TelNo))
                          .ToListAsync();

            if (existingEmails.Contains(model.TelNo))
            {
                ModelState.AddModelError("TelNO", "Bu Numara zaten kullanılıyor.");
                return View(model);
            }

            var Musteri = new Musteri
            {
                Ad=model.Ad,
                Soyad=model.Soyad,
                Email=model.Email,
                Sifre=model.Sifre,
                TelNo=model.TelNo,
                Cinsiyet=model.Cinsiyet,
            };

            _dataContext.Musteriler.Add(Musteri);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
