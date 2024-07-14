using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepository;

        public PostController(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.postRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = this.postRepository.GetById(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Post post)
        {
            this.postRepository.Create(post);
            return CreatedAtAction(nameof(Get), new { id = post.PostId }, post);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Post post)
        {
            if (id != post.PostId) return BadRequest();
            this.postRepository.Update(post);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.postRepository.Delete(id);
            return NoContent();
        }
    }
}