namespace DataAccess.Repositories.Interfaces
{
    using System.Collections.Generic;
    using DataAccess.Models;

    public interface ICategoryRepository
    {
        void           Create(Category category);
        Category       Read(int id);
        void           Update(Category category);
        void           Delete(int id);
        List<Category> GetAll();
    }
}