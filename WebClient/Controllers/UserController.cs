namespace WebClient.Controllers
{
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using DataAccess.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using WebClient.Services;

    public class UserController : Controller
    {
        private readonly HttpClient          httpClient;
        private readonly UserValidateService userValidateService;
        private readonly PostService         postService;
        private readonly ReplyService        replyService;
        private readonly UserService         userService;

        public UserController(HttpClient httpClient,
            UserValidateService userValidateService,
            PostService postService,
            ReplyService replyService,
            UserService userService)
        {
            this.httpClient          = httpClient;
            this.userValidateService = userValidateService;
            this.postService         = postService;
            this.replyService        = replyService;
            this.userService         = userService;
        }

        public IActionResult Index() { return this.RedirectToAction(nameof(Login)); }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.userValidateService.IsUserAuthenticated()) return this.RedirectToAction("Index", "Home");
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

        [HttpGet]
        public IActionResult Register()
        {
            if (this.userValidateService.IsUserAuthenticated()) return this.RedirectToAction("Index", "Home");
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.RoleId    = 3;
            var isRegisterSucceeded = await this.RegisterPostAsync(user);
            if (isRegisterSucceeded)
            {
                return this.RedirectToAction("Login");
            }

            return this.RedirectToAction("RegisterPost");
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile(int option)
        {
            var claimPrincipal = this.userValidateService.GetClaimPrincipal();
            var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var user = await this.GetUserById(int.Parse(userId));
            switch (option)
            {
                case 0:
                {
                    if (user != null)
                    {
                        var posts = await this.postService.GetPostsByUserIdAsync(user.UserId);
                        this.ViewBag.Posts = posts;
                        var replies = await this.replyService.GetRepliesByUserId(user.UserId);
                        if (replies != null)
                        {
                            var tasks = new List<Task>();
                            foreach (var reply in replies)
                            {
                                tasks.Add(this.AddPostToReply(reply));
                            }

                            await Task.WhenAll(tasks);
                        }

                        this.ViewBag.Replies = replies;
                    }

                    break;
                }

                case 1:
                {
                    if (user != null)
                    {
                        var posts = await this.postService.GetPostsByUserIdAsync(user.UserId);
                        this.ViewBag.Posts   = posts;
                        this.ViewBag.Replies = null;
                    }

                    break;
                }

                case 2:
                    if (user != null)
                    {
                        this.ViewBag.Posts = null;
                        var replies = await this.replyService.GetRepliesByUserId(user.UserId);
                        this.ViewBag.Replies = replies;
                    }

                    break;
            }

            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticated();
            this.ViewBag.User            = user;

            return this.View();
        }

        public async Task AddPostToReply(Reply reply)
        {
            var postId = reply.PostId;
            if (postId != null)
            {
                var postIdInt = (int)postId;
                var post      = await this.postService.GetPostByIdAsync(postIdInt);
                reply.Post = post;
            }
        }
        
        [HttpGet]
        public IActionResult Logout()
        {
            if (!this.userValidateService.IsUserAuthenticated()) return this.RedirectToAction("Index", "Home");
            this.Response.Cookies.Delete("AuthToken");
            return this.RedirectToAction("Login");
        }

        private async Task<User?> GetUserById(int id)
        {
            var response = await this.httpClient.GetAsync($"http://localhost:5000/api/User/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user         = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
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

        [HttpGet]
        public async Task<IActionResult> ChangeProfile()
        {
            var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
            if (claimsPrincipal == null) return this.RedirectToAction("Index", "Home");
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user   = await this.userService.GetUserById(int.Parse(userId));
            this.ViewBag.User = user;
            
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfile(User user)
        {
            var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
            var userId          = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.userService.UpdateUsername(int.Parse(userId), user.UserName);

            return this.RedirectToAction("ViewProfile");
        }
    }
}