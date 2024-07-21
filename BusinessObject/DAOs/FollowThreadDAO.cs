namespace DataAccess.DAOs;

using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

public class FollowThreadDAO
{
    private readonly ForumDBContext context;

    public FollowThreadDAO(ForumDBContext context) { this.context = context; }

    public FollowThread GetFollowThreadByUserId(int userId)
    {
        return this.context.FollowThreads
            .Include(ft => ft.Thread)
            .Include(ft => ft.User)
            .FirstOrDefault(ft => ft.UserId == userId);
    }

    public List<FollowThread> GetAllFollowThreads()
    {
        return this.context.FollowThreads
            .Include(ft => ft.Thread)
            .Include(ft => ft.User)
            .ToList();
    }

    public void AddFollowThread(FollowThread followThread)
    {
        this.context.FollowThreads.Add(followThread);
        this.context.SaveChanges();
    }

    public void UpdateFollowThread(FollowThread followThread)
    {
        this.context.FollowThreads.Update(followThread);
        this.context.SaveChanges();
    }

    public void DeleteFollowThread(int followId)
    {
        var followThread = this.context.FollowThreads.Find(followId);
        if (followThread != null)
        {
            this.context.FollowThreads.Remove(followThread);
            this.context.SaveChanges();
        }
    }
}