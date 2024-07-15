namespace WebClient.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Thread = DataAccess.Models.Thread;

public class ThreadController : Controller
{
    private readonly HttpClient httpClient;

    public ThreadController(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Create(string message)
    {
        var claimsPrincipal = this.GetClaimPrincipal();
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
        var claimPrincipal = this.GetClaimPrincipal();
        var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        thread.UserId   = int.Parse(userId);
        thread.CreatedAt = DateTime.Now;
        var jsonContent = JsonConvert.SerializeObject(thread);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await this.httpClient.PostAsync("http://localhost:5000/api/Thread", content);

        if (response.IsSuccessStatusCode)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return this.RedirectToAction("Create", new {message = "Error"});
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
}