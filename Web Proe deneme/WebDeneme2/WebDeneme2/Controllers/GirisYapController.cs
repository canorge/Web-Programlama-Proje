using Microsoft.AspNetCore.Mvc;

namespace WebDeneme2.Controllers
{
    public class GirisYapController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
