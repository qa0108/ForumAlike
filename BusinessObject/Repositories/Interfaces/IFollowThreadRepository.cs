using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IFollowThreadRepository
{
    FollowThread       GetById(int followId);
    List<FollowThread> GetAll();
    void       Add(int userId, int threadId);
    FollowThread       Update(FollowThread followThread);
    void               Delete(int userId, int threadId);
    FollowThread       GetFollowThreadByUserId(int userId);
}