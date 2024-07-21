namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class PostDAO
    {
        private readonly ForumDBContext context;

        public PostDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Create(Post post)
        {
            this.context.Posts.Add(post);
            this.context.SaveChanges();
        }

        public Post? GetById(int id)
        {
            return context.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .FirstOrDefault(p => p.PostId == id);
        }

        public void Update(Post post)
        {
            context.Posts.Update(post);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var post = context.Posts.Find(id);
            if (post != null)
            {
                context.Posts.Remove(post);
                context.SaveChanges();
            }
        }

        public List<Post> GetAll()
        {
            return this.context.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .ToList();
        }
    }
}