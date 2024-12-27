using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebDeneme2.Models;

namespace WebDeneme2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://www.ailabapi.com/api/portrait/effects/hairstyle-editor";        
        private const string ApiKey = "BheWkt2wQCLCX9AySOo0Tt6NdzD5ZqiYFcO9FwSHq4ysR3j1U5KYrxeE37Maivu7"; // RapidAPI Anahtarýnýz

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

        [HttpPost]
        public async Task<IActionResult> AI(IFormFile photo, string hair_style, string color)
        {
            if (photo == null || photo.Length == 0)
            {
                ViewBag.Error = "Lütfen bir fotoðraf yükleyin.";
                return View();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ApiUrl),
                Headers =
                {
                    { "ailabapi-api-key", ApiKey },  // API Anahtarý
                },
            };

            using (var content = new MultipartFormDataContent())
            {
                using (var fileStream = photo.OpenReadStream())
                {
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(photo.ContentType);
                    content.Add(fileContent, "image_target", photo.FileName);

                    content.Add(new StringContent("async"), "task_type");
                    content.Add(new StringContent("1"), "auto"); // Mode
                    content.Add(new StringContent(hair_style), "hair_style");
                    if (!string.IsNullOrEmpty(color))
                    {
                        content.Add(new StringContent(color), "color");
                    }

                    request.Content = content;

                    using (var response = await client.SendAsync(request))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            ViewBag.Error = $"Bir hata oluþtu: {response.ReasonPhrase} - {errorResponse}";
                            return View();
                        }
                        Console.WriteLine(response);
                        var body = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        var responseJson = JsonDocument.Parse(body);
                        Console.WriteLine(responseJson);
                        var images = responseJson.RootElement
                          .GetProperty("data")
                          .GetProperty("image")
                          .GetString();
                        
                        if (!String.IsNullOrEmpty(images))
                        {
                            Console.WriteLine(images);
                                ViewBag.AIResult = "data:image/jpeg;base64,"+ images;
                            
                        }
                        else
                        {
                            ViewBag.Error = "Resim iþlenirken bir hata oluþtu.";
                        }
                    }
                }
            }

            return View();
        }

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

