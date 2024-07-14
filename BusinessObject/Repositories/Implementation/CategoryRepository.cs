namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO categoryDAO;

        public CategoryRepository(CategoryDAO categoryDAO)
        {
            this.categoryDAO = categoryDAO;
        }

        public void Create(Category category)
        {
            this.categoryDAO.Add(category);
        }

        public Category? GetById(int id)
        {
            return this.categoryDAO.GetById(id);
        }

        public void Update(Category category)
        {
            this.categoryDAO.Update(category);
        }

        public void Delete(int id)
        {
            this.categoryDAO.Delete(id);
        }

        public List<Category> GetAll()
        {
            return this.categoryDAO.GetAll();
        }
    }
}