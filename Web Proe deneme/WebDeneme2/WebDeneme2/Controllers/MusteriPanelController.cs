using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;



namespace WebDeneme2.Controllers
{
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
            return View(musteri);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Musteri model)
        {
            if (ModelState.IsValid)
            {
                var musteri = await _dataContext.Musteriler.FirstOrDefaultAsync(m=>m.Id==model.Id);
                if (musteri == null )
                {
                    return NotFound();
                }
                    musteri.Ad = model.Ad;
                    musteri.Soyad = model.Soyad;
                    musteri.Email = model.Email;
                    musteri.TelNo = model.TelNo;

                    _dataContext.Musteriler.Update(musteri);
                    _dataContext.SaveChanges();
                    return RedirectToAction("Index", new { id = musteri.Id });
            }
            return RedirectToAction("Index", new { id = model.Id });
        }

        public async Task<IActionResult> Randevular(int id)
        {
            var musteri=await _dataContext.Musteriler
                                                     .Include(m=> m.Randevular)
                                                     .ThenInclude(m=>m.Calisan)
                                                     .FirstOrDefaultAsync(m=>m.Id==id);
            if(musteri == null)
            {
                return NotFound();
            }
            return View(musteri);   
        }

    }
}

         
