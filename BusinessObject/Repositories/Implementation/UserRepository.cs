namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class UserRepository : IUserRepository
    {
        private readonly UserDAO userDAO;

        public UserRepository(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        public void Create(User user)
        {
            this.userDAO.Create(user);
        }

        public User GetById(int id)
        {
            return this.userDAO.GetById(id);
        }

        public void Update(User user)
        {
            this.userDAO.Update(user);
        }

        public void Delete(int id)
        {
            this.userDAO.Delete(id);
        }

        public List<User> GetAll()
        {
            return this.userDAO.GetAll();
        }
    }
}