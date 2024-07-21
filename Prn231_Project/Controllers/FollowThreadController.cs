namespace Prn231_Project.Controllers;

using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
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

    [HttpDelete("{userId}/{threadId}")]
    public IActionResult Delete(int userId, int threadId)
    {
        this.followThreadRepository.Delete(userId,threadId);
        return NoContent();
    }


    [HttpPost]
    public IActionResult Add([FromBody] FollowThread followThread)
    {
        this.followThreadRepository.Add(followThread.UserId, followThread.ThreadId);
        return this.Ok();
    }
}