namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class ThreadRepository : IThreadRepository
    {
        private readonly ThreadDAO threadDAO;

        public ThreadRepository(ThreadDAO threadDAO)
        {
            this.threadDAO = threadDAO;
        }

        public void Create(Thread thread)
        {
            this.threadDAO.Create(thread);
        }

        public Thread GetById(int id)
        {
            return this.threadDAO.GetById(id);
        }

        public void Update(Thread thread)
        {
            this.threadDAO.Update(thread);
        }

        public void Delete(int id)
        {
            this.threadDAO.Delete(id);
        }

        public List<Thread> GetAll()
        {
            return this.threadDAO.GetAll();
        }
    }
}