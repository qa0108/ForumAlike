namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class PostRepository : IPostRepository
    {
        private readonly PostDAO postDAO;

        public PostRepository(PostDAO postDAO)
        {
            this.postDAO = postDAO;
        }

        public void Create(Post post)
        {
            this.postDAO.Create(post);
        }

        public Post? GetById(int id)
        {
            return this.postDAO.GetById(id);
        }

        public void Update(Post post)
        {
            this.postDAO.Update(post);
        }

        public void Delete(int id)
        {
            this.postDAO.Delete(id);
        }

        public List<Post> GetAll()
        {
            return this.postDAO.GetAll();
        }
    }
}