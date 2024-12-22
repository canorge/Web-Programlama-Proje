using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebDeneme2.Controllers
{
    // API yolu "api/HizmetApi" olacak
    [Route("api/[controller]")]
    [ApiController]
    public class HizmetApiController : ControllerBase
    {
        private readonly DataContext _context;

        // Constructor - Veritabanı context'ini enjekte ediyoruz
        public HizmetApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HizmetApi - Hizmetlerin listesini JSON formatında döner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hizmet>>> GetHizmetler()
        {
            // Hizmetler tablosundaki tüm verileri alıyoruz
            var hizmetler = await _context.Hizmetler.ToListAsync();

            if (hizmetler == null || hizmetler.Count == 0)
            {
                return NotFound();  // Hizmet bulunamadığında 404 döner
            }

            return Ok(hizmetler);  // Hizmetlerin listesini JSON formatında döner
        }
    }
}
