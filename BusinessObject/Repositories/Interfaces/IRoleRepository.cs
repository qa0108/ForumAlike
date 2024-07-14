namespace DataAccess.Repositories.Interfaces;

using DataAccess.Models;

public interface IRoleRepository
{
    void       Create(Role role);
    Role       Read(int id);
    void       Update(Role role);
    void       Delete(int id);
    List<Role> GetAll();
}