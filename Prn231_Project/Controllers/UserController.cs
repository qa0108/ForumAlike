namespace Prn231_Project.Controllers
{
    using DataAccess.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ForumDBContext context;

        public UserController(ForumDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult CheckLogin(string email, string password)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Email.Equals(email)
           && u.PasswordHash.Equals(password));

            if (user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpGet("GetUserByEmail")]
        public IActionResult GetUserByEmail(string email) {
            var user = this.context.Users.FirstOrDefault(u => u.Email.Equals(email));

            if (user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (user == null) { return BadRequest("User cannot be null"); }

            if (this.context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("This email has been used");
            }

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            if (user == null) { return BadRequest("User cannot be null"); }

            var targetUser = this.context.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (targetUser == null) return BadRequest($"Cannot find user with username: {user.UserName}");

            targetUser.PasswordHash = user.PasswordHash;
            targetUser.Email = user.Email;
            targetUser.RoleId = user.RoleId;
            this.context.Users.Update(targetUser);

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var targetUser = this.context.Users.FirstOrDefault(u => u.UserId == id);

            if (targetUser == null) return BadRequest($"Cannot find user with id: {id}");

            this.context.Users.Remove(targetUser);

            return Ok(targetUser);
        }
    }
}
