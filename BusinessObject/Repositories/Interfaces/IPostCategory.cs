namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IPostRepository
{
    void       Create(Post post);
    Post       GetById(int id);
    void       Update(Post post);
    void       Delete(int id);
    List<Post> GetAll();
}