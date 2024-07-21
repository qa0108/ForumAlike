namespace DataAccess.Repositories.Implementation;

using DataAccess.DAOs;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

public class FollowThreadRepository : IFollowThreadRepository
{
    private readonly FollowThreadDAO followThreadDao;

    public FollowThreadRepository(FollowThreadDAO followThreadDao)
    {
        this.followThreadDao = followThreadDao;
    }

    public FollowThread Add(FollowThread followThread)
    {
        this.followThreadDao.AddFollowThread(followThread);
        return followThread;
    }

    public void Delete(int followId)
    {
        this.followThreadDao.DeleteFollowThread(followId);
    }

    public List<FollowThread> GetAll()
    {
        return this.followThreadDao.GetAllFollowThreads();
    }

    public FollowThread GetById(int userId)
    {
        return this.followThreadDao.GetFollowThreadByUserId(userId);
    }

    public FollowThread Update(FollowThread followThread)
    {
        this.followThreadDao.UpdateFollowThread(followThread);
        return followThread;
    }

    public FollowThread GetFollowThreadByUserId(int userId)
    {
        return this.followThreadDao.GetFollowThreadByUserId(userId);
    }
}