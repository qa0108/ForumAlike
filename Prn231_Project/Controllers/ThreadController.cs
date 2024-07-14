using Thread = DataAccess.Models.Thread;

namespace Prn231_Project.Controllers
{
    using DataAccess.Repositories.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OData.Query;

    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadRepository threadRepository;

        public ThreadController(IThreadRepository threadRepository)
        {
            this.threadRepository = threadRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.threadRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var thread = this.threadRepository.GetById(id);
            if (thread == null) return this.NotFound();
            return this.Ok(thread);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Thread thread)
        {
            this.threadRepository.Create(thread);
            return CreatedAtAction(nameof(Get), new { id = thread.ThreadId }, thread);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Thread thread)
        {
            if (id != thread.ThreadId) return BadRequest();
            this.threadRepository.Update(thread);
            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            this.threadRepository.Delete(id);
            return this.NoContent();
        }
    }
}