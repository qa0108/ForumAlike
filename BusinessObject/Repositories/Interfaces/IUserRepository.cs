namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IUserRepository
{
    void       Create(User user);
    User       Read(int id);
    void       Update(User user);
    void       Delete(int id);
    List<User> GetAll();
}