namespace DataAccess.DAOs
{
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class ReplyDAO
    {
        private readonly ForumDBContext context;

        public ReplyDAO(ForumDBContext context)
        {
            this.context = context;
        }

        public void Create(Reply reply)
        {
            context.Replies.Add(reply);
            context.SaveChanges();
        }

        public Reply? GetById(int id)
        {
            return context.Replies
                .Include(r => r.User)
                .Include(r => r.Post)
                .FirstOrDefault(r => r.ReplyId == id);
        }

        public void Update(Reply reply)
        {
            context.Replies.Update(reply);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var reply = context.Replies.Find(id);
            if (reply != null)
            {
                context.Replies.Remove(reply);
                context.SaveChanges();
            }
        }

        public List<Reply> GetAll()
        {
            return this.context.Replies
                .Include(r => r.User)
                .Include(r => r.Post)
                .ToList();
        }
    }
}