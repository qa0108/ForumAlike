namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserDAO
    {
        private readonly ForumDBContext context;

        public UserDAO(ForumDBContext context) { this.context = context; }

        public void Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User? GetById(int id) { return context.Users.Find(id); }

        public void Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        public List<User> GetAll() { return context.Users.ToList(); }
    }
}