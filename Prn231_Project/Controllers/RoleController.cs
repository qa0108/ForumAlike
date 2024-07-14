using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.roleRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var role = this.roleRepository.GetById(id);
            if (role == null) return NotFound();
            return this.Ok(role);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Role role)
        {
            this.roleRepository.Create(role);
            return this.CreatedAtAction(nameof(Get), new { id = role.RoleId }, role);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Role role)
        {
            if (id != role.RoleId) return BadRequest();
            this.roleRepository.Update(role);
            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            this.roleRepository.Delete(id);
            return this.NoContent();
        }
    }
}