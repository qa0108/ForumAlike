using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClient.Models;
using DataAccess.Models;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly HttpClient              httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            this.logger     = logger;
            this.httpClient = httpClient;
        }

        // Update HomeController.cs
        public async Task<IActionResult> Index()
        {
            var posts                             = await this.GetAllPosts();
            if (posts != null) this.ViewBag.Posts = posts;
            return this.View();
        }

        public IActionResult Privacy() { return this.View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public async Task<List<Post>?> GetAllPosts()
        {
            var response = await this.httpClient.GetAsync("http://localhost:5000/odata/Post");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var posts        = JsonConvert.DeserializeObject<List<Post>>(jsonResponse);
                return posts;
            }

            return null;
        }
    }
}