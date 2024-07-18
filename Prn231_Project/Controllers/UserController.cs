using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using Prn231_Project.DTOs;
    using Prn231_Project.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly UserService     userService;

        public UserController(IUserRepository userRepository, UserService userService)
        {
            this.userRepository = userRepository;
            this.userService    = userService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get() { return this.Ok(this.userRepository.GetAll()); }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var user = this.userRepository.GetById(id);
            if (user == null) return this.NotFound();
            return this.Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            this.userRepository.Create(user);
            return this.CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if (id != user.UserId) return this.BadRequest();
            this.userRepository.Update(user);
            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            this.userRepository.Delete(id);
            return this.NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            var user         = this.userService.ValidateUser(userLogin.Email, userLogin.Password);

            if (user == null) return this.Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key          = Encoding.UTF8.GetBytes(Config.JWT_SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Role, user.RoleId.ToString()),
                    new(ClaimTypes.Email, user.Email),
                    new("UserId", user.UserId.ToString())
                }),
                Expires            = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token       = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return this.Ok(new { Token = tokenString });

        }
    }
}