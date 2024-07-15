namespace WebClient.Controllers
{
    using System.Net;
    using System.Text;
    using DataAccess.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class UserController : Controller
    {
        private readonly HttpClient httpClient;
        
        private const    string     BaseApiLink = "http://localhost:5000/api/User";

        public UserController(HttpClient httpClient) { this.httpClient = httpClient; }
        
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginDto    = new { Email = email, Password = password };
            var jsonContent = JsonConvert.SerializeObject(loginDto);
            var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync("http://localhost:5000/api/User/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var token          = JsonConvert.DeserializeObject<dynamic>(responseString)?.token;
                this.Response.Cookies.Append("AuthToken", token.ToString(), new CookieOptions { HttpOnly = true, Secure = true });
                return this.RedirectToAction("Index", "Home");
            }

            return this.View("Error");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.RoleId = 3;
            var isRegisterSucceeded = await this.RegisterPostAsync(user);
            if (isRegisterSucceeded)
            {
                return this.RedirectToAction("Login");
            }

            return this.RedirectToAction("Register");
        }

        public async Task<bool> RegisterPostAsync(User user)
        {
            const string link = "http://localhost:5000/api/User/";

            {
                using (var res = await this.httpClient.PostAsJsonAsync(link, user))
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
