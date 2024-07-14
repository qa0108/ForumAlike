namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IReplyRepository
{
    public interface IPostRepository
    {
        void        Create(Reply reply);
        Reply       Read(int id);
        void        Update(Reply reply);
        void        Delete(int id);
        List<Reply> GetAll();
    }
}