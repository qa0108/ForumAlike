namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RoleDAO
    {
        private readonly ForumDBContext context;

        public RoleDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Create(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }

        public Role? GetById(int id)
        {
            return context.Roles.Find(id);
        }

        public void Update(Role role)
        {
            context.Roles.Update(role);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var role = context.Roles.Find(id);
            if (role != null)
            {
                context.Roles.Remove(role);
                context.SaveChanges();
            }
        }

        public List<Role> GetAll()
        {
            return context.Roles.ToList();
        }
    }
}