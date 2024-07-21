using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IFollowThreadRepository
{
    FollowThread       GetById(int followId);
    List<FollowThread> GetAll();
    FollowThread       Add(FollowThread followThread);
    FollowThread       Update(FollowThread followThread);
    void               Delete(int followId);
    FollowThread       GetFollowThreadByUserId(int userId);
}