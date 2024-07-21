namespace Prn231_Project.Controllers;

using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class FollowThreadController : ControllerBase
{
    private readonly IFollowThreadRepository followThreadRepository;

    public FollowThreadController(IFollowThreadRepository followThreadRepository) { this.followThreadRepository = followThreadRepository; }

    [HttpGet]
    public IActionResult GetAll()
    {
        var followThreads = this.followThreadRepository.GetAll();
        if (!followThreads.Any())
        {
            return NotFound("No follow threads found.");
        }

        return Ok(followThreads);
    }

    [HttpGet("{userId}")]
    public IActionResult GetById(int userId)
    {
        var followThread = this.followThreadRepository.GetFollowThreadByUserId(userId);
        if (followThread == null)
        {
            return NotFound($"Follow thread with ID {userId} not found.");
        }

        return Ok(followThread);
    }
    
    [HttpPost]
    public IActionResult Add([FromBody] FollowThread followThread)
    {
        var addedThread = this.followThreadRepository.Add(followThread);
        if (addedThread == null)
        {
            return BadRequest("Unable to add follow thread.");
        }

        return CreatedAtAction(nameof(GetAll), new { id = addedThread.FollowId }, addedThread);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        this.followThreadRepository.Delete(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] FollowThread followThread)
    {
        if (id != followThread.FollowId)
        {
            return BadRequest("Mismatched follow thread ID.");
        }

        this.followThreadRepository.Update(followThread);
        return NoContent();
    }
}