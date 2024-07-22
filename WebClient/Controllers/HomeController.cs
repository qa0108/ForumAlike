namespace WebClient.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using WebClient.Models;
    using WebClient.Services;

    public class HomeController : Controller
    {
        private readonly UserValidateService userValidateService;
        private readonly PostService         postService;
        private readonly UserService         userService;
        private readonly ThreadService       threadService;

        public HomeController(
            UserValidateService userValidateService,
            PostService postService,
            UserService userService,
            ThreadService threadService)
        {
            this.userValidateService = userValidateService;
            this.postService         = postService;
            this.userService         = userService;
            this.threadService       = threadService;
        }

        public async Task<IActionResult> Index()
        {
            var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
            if (claimsPrincipal != null)
            {
                var userRole = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole != null && int.Parse(userRole) == 1)
                {
                    return this.RedirectToAction("Index", "Admin");
                }

                var userId        = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var followThreads = await this.userService.GetFollowThreads(int.Parse(userId));
                this.ViewBag.FollowThreads = followThreads;
            }

            this.ViewBag.IsAuthenticated = this.userValidateService.IsUserAuthenticated();
            var posts                             = await this.postService.GetAllPosts();
            if (posts != null) this.ViewBag.Posts = posts;
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchContent)
        {
            this.ViewBag.SearchContent = searchContent;
            var users = await this.userService.GetAllUsers();
            this.ViewBag.Users = users?.Where(u => u.UserName.ToLower().Contains(searchContent.ToLower())).ToList();
            var threads = await this.threadService.GetAllThreads();
            this.ViewBag.Threads = threads?.Where(t => t.Title.ToLower().Contains(searchContent.ToLower())).ToList();
            var posts = await this.postService.GetAllPosts();
            this.ViewBag.Posts = posts?.Where(p => p.Title.ToLower().Contains(searchContent.ToLower())).ToList();
            return this.View();
        }
        
        public IActionResult Privacy() { return this.View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() { return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier }); }
    }
}