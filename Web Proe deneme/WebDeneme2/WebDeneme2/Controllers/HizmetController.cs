using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class HizmetController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // HizmetController'a HttpClientFactory enjekte ediyoruz
        public HizmetController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Hizmet verilerini almak için API'yi çağırıyoruz
        public async Task<IActionResult> Hizmet()
        {
            var client = _httpClientFactory.CreateClient();

            // API'den hizmetleri alıyoruz
            var response = await client.GetFromJsonAsync<IEnumerable<Hizmet>>("https://localhost:7195/api/HizmetApi");
            return View(response);
        }
    }
}
