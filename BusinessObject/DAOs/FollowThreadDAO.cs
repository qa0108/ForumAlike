namespace DataAccess.DAOs;

using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

public class FollowThreadDAO
{
    private readonly ForumDBContext context;

    public FollowThreadDAO(ForumDBContext context) { this.context = context; }

    public List<FollowThread> GetFollowThreadByUserId(int userId)
    {
        return this.context.FollowThreads
            .Include(ft => ft.Thread)
            .Include(ft => ft.User)
            .Where(ft => ft.UserId == userId)
            .ToList();
    }

    public List<FollowThread> GetAllFollowThreads()
    {
        return this.context.FollowThreads
            .Include(ft => ft.Thread)
            .Include(ft => ft.User)
            .ToList();
    }

    public void AddFollowThread(int userId, int threadId)
    {
        var user   = context.Users.Find(userId);
        var thread = context.Threads.Find(threadId);

        if (user != null && thread != null)
        {
            var followThread = new FollowThread
            {
                UserId     = user.UserId,
                ThreadId   = thread.ThreadId,
                FollowedAt = DateTime.Now,
                User       = user,
                Thread     = thread
            };

            context.FollowThreads.Add(followThread);
            context.SaveChanges();
        }
        else
        {
            // Handle the case where the user or thread does not exist
            Console.WriteLine("User or Thread not found.");
        }
    }

    public void UpdateFollowThread(FollowThread followThread)
    {
        this.context.FollowThreads.Update(followThread);
        this.context.SaveChanges();
    }

    public void DeleteFollowThread(int userId, int threadId)
    {
        var followThread = this.context.FollowThreads.FirstOrDefault(ft => ft.UserId == userId && ft.ThreadId == threadId);
        if (followThread != null)
        {
            this.context.FollowThreads.Remove(followThread);
            this.context.SaveChanges();
        }
    }
}