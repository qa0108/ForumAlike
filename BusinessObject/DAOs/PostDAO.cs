namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class PostDAO
    {
        private readonly ForumDBContext context;

        public PostDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Create(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public Post? GetById(int id)
        {
            return context.Posts.Find(id);
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
            return context.Posts.ToList();
        }
    }
}