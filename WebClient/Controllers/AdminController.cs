namespace WebClient.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebClient.Services;

public class AdminController : Controller
{
    private readonly UserValidateService userValidateService;
    private readonly PostService         postService;
    private readonly UserService         userService;
    public AdminController(
        UserValidateService userValidateService,
        PostService postService,
        UserService userService)
    {
        this.userValidateService = userValidateService;
        this.postService         = postService;
        this.userService         = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int option)
    {
        var isAuthenticated = this.userValidateService.IsUserAuthenticated();
        this.ViewBag.IsAuthenticated = isAuthenticated;
        if (!isAuthenticated)
        {
            return this.RedirectToAction("Login", "User");
        }

        var claimsPrincipal = this.userValidateService.GetClaimPrincipal();
        var value           = claimsPrincipal?.FindFirst(ClaimTypes.Role)?.Value;
        if (value != null)
        {
            var roleId = int.Parse(value);
            if (roleId != 1) return this.RedirectToAction("Index", "Home");
        }

        switch (option)
        {
            case 0:
            {
                var posts = await this.postService.GetAllPosts();
                this.ViewBag.Posts = posts;
                this.ViewBag.Users = null;

                break;
            }

            case 1:
            {
                var users = await this.userService.GetAllUsers();
                this.ViewBag.Posts = null;
                this.ViewBag.Users = users;

                break;
            }
        }

        return this.View();
    }

    [HttpGet]
    public async Task<IActionResult> Promote(int userId)
    {
        var isAdmin = this.userValidateService.IsAdmin();
        if (!isAdmin)
        {
            return this.RedirectToAction("Index", "Home");
        }

        await this.userService.Promote(userId);
        return this.RedirectToAction("Index",new {option = 1});
    }
    
    [HttpGet]
    public async Task<IActionResult> Demote(int userId)
    {
        var isAdmin = this.userValidateService.IsAdmin();
        if (!isAdmin)
        {
            return this.RedirectToAction("Index", "Home");
        }

        await this.userService.Demote(userId);
        return this.RedirectToAction("Index", new {option = 1});
    }
}