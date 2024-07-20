using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClient.Models;
using DataAccess.Models;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using WebClient.Services;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly HttpClient              httpClient;
        private readonly UserValidateService     userValidateService;

        public HomeController(ILogger<HomeController> logger,
            HttpClient httpClient,
            UserValidateService userValidateService)
        {
            this.logger              = logger;
            this.httpClient          = httpClient;
            this.userValidateService = userValidateService;
        }

        // Update HomeController.cs
        public async Task<IActionResult> Index()
        {
            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticated();
            var posts                             = await this.GetAllPosts();
            if (posts != null) this.ViewBag.Posts = posts;
            return this.View();
        }

        public IActionResult Privacy() { return this.View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() { return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier }); }

        [HttpGet]
        public IActionResult TestAuthen()
        {
            var token = this.HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("Index");
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
                var userRole  = principal.FindFirst(ClaimTypes.Role)?.Value;
                var userName  = principal.FindFirst(ClaimTypes.Name)?.Value;

                // Here you can check the user's role or name and return appropriate response
                return this.View();
            }
            catch (SecurityTokenException)
            {
                return this.RedirectToAction("Index");
            }
        }

        public async Task<List<Post>?> GetAllPosts()
        {
            var response = await this.httpClient.GetAsync("http://localhost:5000/api/Post");

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