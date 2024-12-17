using Microsoft.AspNetCore.Mvc;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class MusteriPanelController :Controller
    {
        public IActionResult Index(Musteri model)
        {
            return View(model);
        }
    }
}
