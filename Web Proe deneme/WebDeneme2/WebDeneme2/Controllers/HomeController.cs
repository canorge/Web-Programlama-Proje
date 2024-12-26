using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebDeneme2.Models;

using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
namespace WebDeneme2.Controllers
{
    public class HomeController : Controller
    {
        // r8_3Cm0dl0MqTcinxlaP3OO5v3ijHwwKUv3xBQfM
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AI()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AI(IFormFile file)
        //{
            
        //}



     


        public IActionResult Iletisim()
        {
            return View();
        }

        
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
