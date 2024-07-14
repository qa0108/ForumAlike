namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoryDAO
    {
        private readonly ForumDBContext context;

        public CategoryDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public void Update(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }
    }
}