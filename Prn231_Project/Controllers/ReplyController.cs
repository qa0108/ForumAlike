using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Prn231_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyRepository replyRepository;

        public ReplyController(IReplyRepository replyRepository)
        {
            this.replyRepository = replyRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return this.Ok(this.replyRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var reply = this.replyRepository.GetById(id);
            if (reply == null) return this.NotFound();
            return this.Ok(reply);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Reply reply)
        {
            this.replyRepository.Create(reply);
            return this.CreatedAtAction(nameof(Get), new { id = reply.ReplyId }, reply);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Reply reply)
        {
            if (id != reply.ReplyId) return BadRequest();
            this.replyRepository.Update(reply);
            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            this.replyRepository.Delete(id);
            return this.NoContent();
        }
    }
}