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

        public HomeController(
            UserValidateService userValidateService,
            PostService postService,
            UserService userService)
        {
            this.userValidateService = userValidateService;
            this.postService         = postService;
            this.userService         = userService;
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
        
        public IActionResult Privacy() { return this.View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() { return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier }); }
    }
}