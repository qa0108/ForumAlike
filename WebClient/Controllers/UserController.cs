using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebClient.ApiModels;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        private const string BaseApiLink = "http://localhost:5000/api/User";

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]  
        public async Task<IActionResult> Login(string email, string password)
        {
            string link = BaseApiLink + $"/GetUserByEmail?email={email}";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(link))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<User>(data);
                        if (user != null && user.PasswordHash == password) {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.RoleId = 3;
            var isRegiserSucceeded = await this.RegisterPostAsync(user);
            if (isRegiserSucceeded)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Register");
        }

        public async Task<bool> RegisterPostAsync(User user)
        {
            var link = "http://localhost:5000/api/User/";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(link, user))
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
