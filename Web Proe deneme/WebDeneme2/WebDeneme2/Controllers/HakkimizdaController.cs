using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class HakkimizdaController : Controller
    {
        private readonly DataContext _dataContext;

        public HakkimizdaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Hakkimizda()
        {
            
            return View();
        }

    }
}
