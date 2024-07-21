namespace WebClient.Controllers;

using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.Services;
using Thread = DataAccess.Models.Thread;

public class ThreadController : Controller
{
    private readonly HttpClient          httpClient;
    private readonly UserValidateService userValidateService;
    private readonly ThreadService       threadService;
    private readonly PostService         postService;
    private readonly UserService         userService;

    public ThreadController(HttpClient httpClient,
        UserValidateService userValidateService,
        ThreadService threadService,
        PostService postService,
        UserService userService)
    {
        this.httpClient          = httpClient;
        this.userValidateService = userValidateService;
        this.threadService       = threadService;
        this.postService         = postService;
        this.userService         = userService;
    }

    [HttpGet]
    public IActionResult Create(string message)
    {
        var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
        if (claimsPrincipal == null)
        {
            return this.RedirectToAction("Login", "User");
        }

        this.ViewBag.WarningMessage = message;
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Thread thread)
    {
        var claimPrincipal = this.userValidateService.GetClaimPrincipal();
        var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        thread.UserId    = int.Parse(userId);
        thread.CreatedAt = DateTime.Now;
        var jsonContent = JsonConvert.SerializeObject(thread);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await this.httpClient.PostAsync("http://localhost:5000/api/Thread", content);

        if (response.IsSuccessStatusCode)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return this.RedirectToAction("Create", new { message = "Error" });
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int threadId)
    {
        var thread = await this.threadService.GetThreadById(threadId);
        if (thread == null) return this.RedirectToAction("Index", "Home");

        var claimPrincipal = this.userValidateService.GetClaimPrincipal();
        if (claimPrincipal != null)
        {
            this.ViewBag.IsAuthenticated = true;
            var userId         = claimPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isFollowThread = await this.threadService.IsUserFollowThread(int.Parse(userId), threadId);
            this.ViewBag.IsFollowed = isFollowThread;
        }

        this.ViewBag.Thread = thread;
        this.ViewBag.Posts  = await this.postService.GetPostByThreadId(threadId);
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Follow(int threadId)
    {
        var claimPrincipal = this.userValidateService.GetClaimPrincipal();
        if (claimPrincipal == null) return this.RedirectToAction("Login", "User");
        var userId         = claimPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user           = await this.userService.GetUserById(int.Parse(userId));
        var thread         = await this.threadService.GetThreadById(threadId);
        var isFollowThread = await this.threadService.IsUserFollowThread(int.Parse(userId), threadId);
        if (isFollowThread)
        {
            await this.threadService.Unfollow(user.UserId, threadId);
        }
        else
        {
            await this.threadService.Follow(user, thread);
        }

        return this.RedirectToAction("Detail", new { threadId });
    }
}