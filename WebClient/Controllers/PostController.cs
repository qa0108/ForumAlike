// WebClient/Controllers/PostController.cs
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Newtonsoft.Json;
using System.Text;
using Thread = DataAccess.Models.Thread;

namespace WebClient.Controllers
{
    public class PostController : Controller
    {
        private readonly HttpClient httpClient;

        public PostController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var threads = await this.GetThreads();
            this.ViewBag.Threads = threads;
            return this.View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(Post newPost)
        {
            newPost.UserId    = 1;
            newPost.CreatedAt = DateTime.Now;
            var jsonContent = JsonConvert.SerializeObject(newPost);
            var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync("http://localhost:5000/api/Post", content);

            if (response.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View("Error");
        }
        
        
        [HttpGet]
        public async Task<List<Thread>> GetThreads()
        {
            var response = await this.httpClient.GetAsync("http://localhost:5000/api/Thread");
            var threads  = new List<Thread>();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                threads = JsonConvert.DeserializeObject<List<Thread>>(jsonResponse);
            }

            return threads;
        }
    }
}