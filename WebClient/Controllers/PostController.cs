// WebClient/Controllers/PostController.cs

using Thread = DataAccess.Models.Thread;

namespace WebClient.Controllers
{
    using System.Security.Claims;
    using System.Text;
    using DataAccess.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using WebClient.Services;

    public class PostController : Controller
    {
        private readonly HttpClient          httpClient;
        private readonly UserValidateService userValidateService;

        public PostController(HttpClient httpClient,
            UserValidateService userValidateService)
        {
            this.httpClient          = httpClient;
            this.userValidateService = userValidateService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string message)
        {
            var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
            if (claimsPrincipal == null)
            {
                return this.RedirectToAction("Login", "User");
            }

            var threads = await this.GetThreads();
            this.ViewBag.Threads         = threads;
            this.ViewBag.WarningMessage  = message;
            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticate();
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Post newPost)
        {
            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticate();
            if (newPost.ThreadId == 0)
            {
                return this.RedirectToAction("Create", new { message = "Select a thread" });
            }

            var claimPrincipal = this.userValidateService.GetClaimPrincipal();
            var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            newPost.UserId    = int.Parse(userId);
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

        [HttpGet]
        public async Task<IActionResult> ViewOwnPosts()
        {
            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticate();
            var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
            var userId          = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return this.View("Error");
            var posts = await this.GetPostByUserId(int.Parse(userId));
            this.ViewBag.OwnPosts = posts;

            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int postId)
        {
            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticate();
            var response = await this.httpClient.GetAsync($"http://localhost:5000/api/Post/{postId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var post        = JsonConvert.DeserializeObject<Post>(jsonResponse);
                this.ViewBag.Post = post;
                return this.View();
            }

            return this.View("Error");
        }
        
        public async Task<List<Post>?> GetPostByUserId(int userId)
        {
            var response = await this.httpClient.GetAsync("http://localhost:5000/api/Post");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var posts        = JsonConvert.DeserializeObject<List<Post>>(jsonResponse);
                if (posts != null) return posts.Where(p => p.UserId == userId).ToList();
            }

            return null;
        }
    }
}