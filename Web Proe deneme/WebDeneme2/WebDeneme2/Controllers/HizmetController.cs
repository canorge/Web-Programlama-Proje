using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class HizmetController : Controller
    {

        private readonly DataContext _context;

        public HizmetController( DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Hizmet()
        {
            var hizmetler = await _context.Hizmetler.ToListAsync();
            return View(hizmetler);
        }

    }
}
