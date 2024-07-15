// WebClient/Controllers/PostController.cs

using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Newtonsoft.Json;
using System.Text;
using Thread = DataAccess.Models.Thread;

namespace WebClient.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;

    public class PostController : Controller
    {
        private readonly HttpClient httpClient;

        public PostController(HttpClient httpClient) { this.httpClient = httpClient; }

        [HttpGet]
        public async Task<IActionResult> Create(string message)
        {
            var claimsPrincipal = this.GetClaimPrincipal();
            if (claimsPrincipal == null)
            {
                return this.RedirectToAction("Login", "User");
            }

            var threads = await this.GetThreads();
            this.ViewBag.Threads        = threads;
            this.ViewBag.WarningMessage = message;
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Post newPost)
        {
            if (newPost.PostId == 0)
            {
                return this.RedirectToAction("Create",new { message = "Select a thread" });
            }
            
            var claimPrincipal = this.GetClaimPrincipal();
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

        public ClaimsPrincipal? GetClaimPrincipal()
        {
            var token = this.HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JWT_SECRET_KEY)),
                ValidateIssuer           = false,
                ValidateAudience         = false,
                // Set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
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