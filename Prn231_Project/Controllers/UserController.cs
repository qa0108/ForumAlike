using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.userRepository.GetAll());
        }

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
    }
}