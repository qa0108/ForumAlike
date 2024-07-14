namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class RoleRepository : IRoleRepository
    {
        private readonly RoleDAO roleDAO;

        public RoleRepository(RoleDAO roleDAO)
        {
            this.roleDAO = roleDAO;
        }

        public void Create(Role role)
        {
            this.roleDAO.Create(role);
        }

        public Role GetById(int id)
        {
            return this.roleDAO.GetById(id);
        }

        public void Update(Role role)
        {
            this.roleDAO.Update(role);
        }

        public void Delete(int id)
        {
            this.roleDAO.Delete(id);
        }

        public List<Role> GetAll()
        {
            return this.roleDAO.GetAll();
        }
    }
}