namespace DataAccess.Repositories.Implementation
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Interfaces;
    using System.Collections.Generic;

    public class ReplyRepository : IReplyRepository
    {
        private readonly ReplyDAO replyDAO;

        public ReplyRepository(ReplyDAO replyDAO)
        {
            this.replyDAO = replyDAO;
        }

        public void Create(Reply reply)
        {
            this.replyDAO.Create(reply);
        }

        public Reply? GetById(int id)
        {
            return this.replyDAO.GetById(id);
        }

        public void Update(Reply reply)
        {
            this.replyDAO.Update(reply);
        }

        public void Delete(int id)
        {
            this.replyDAO.Delete(id);
        }

        public List<Reply> GetAll()
        {
            return this.replyDAO.GetAll();
        }
    }
}