namespace WebClient.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class UserValidateService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserValidateService(IHttpContextAccessor httpContextAccessor) { this.httpContextAccessor = httpContextAccessor; }

    public ClaimsPrincipal? GetClaimPrincipal()
    {
        var token = this.httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
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

    public bool IsUserAuthenticated()
    {
        var claimPrincipal = this.GetClaimPrincipal();
        var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId != null;
    }

    public bool IsAdmin()
    {
        var claimPrincipal = this.GetClaimPrincipal();
        var roleId         = claimPrincipal?.FindFirst(ClaimTypes.Role)?.Value;
        if (roleId == null) return false;
        return int.Parse(roleId) == 1;
    }

    public int? GetUserId()
    {
        var userId = GetClaimPrincipal()?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId != null ? int.Parse(userId) : (int?)null;
    }
}