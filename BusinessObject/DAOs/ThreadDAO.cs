namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreadDAO
    {
        private readonly ForumDBContext context;

        public ThreadDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Create(Thread thread)
        {
            context.Threads.Add(thread);
            context.SaveChanges();
        }

        public Thread GetById(int id)
        {
            return context.Threads.Find(id);
        }

        public void Update(Thread thread)
        {
            context.Threads.Update(thread);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var thread = context.Threads.Find(id);
            if (thread != null)
            {
                context.Threads.Remove(thread);
                context.SaveChanges();
            }
        }

        public List<Thread> GetAll()
        {
            return context.Threads.ToList();
        }
    }
}