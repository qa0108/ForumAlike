using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.categoryRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var category = this.categoryRepository.GetById(id);
            if (category == null) return NotFound();
            return this.Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            this.categoryRepository.Create(category);
            return this.CreatedAtAction(nameof(Get), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            if (id != category.CategoryId) return BadRequest();
            this.categoryRepository.Update(category);
            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            this.categoryRepository.Delete(id);
            return this.NoContent();
        }
    }
}