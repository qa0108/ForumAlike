namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IUserRepository
{
    void  Create(User user);
    User? GetById(int id);
    User? FindUserByUsername(string username);
    User? FindUserByEmail(string email);
    void       Update(User user);
    void       Delete(int id);
    List<User> GetAll();
}