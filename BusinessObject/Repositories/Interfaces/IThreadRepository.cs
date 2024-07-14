namespace DataAccess.Repositories.Interfaces;

using Thread = DataAccess.Models.Thread;

public interface IThreadRepository
{
    void         Create(Thread thread);
    Thread       Read(int id);
    void         Update(Thread thread);
    void         Delete(int id);
    List<Thread> GetAll();
}