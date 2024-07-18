namespace WebClient.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class UserValidateService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserValidateService(IHttpContextAccessor  httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

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

    public bool IsUserAuthenticate()
    {
        var claimPrincipal = this.GetClaimPrincipal();
        var userId         = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId != null;
    }
}