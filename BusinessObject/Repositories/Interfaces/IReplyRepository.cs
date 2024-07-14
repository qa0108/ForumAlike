namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IReplyRepository
{
    void Create(Reply reply);
    Reply?       GetById(int id);
    void        Update(Reply reply);
    void        Delete(int id);
    List<Reply> GetAll();
}