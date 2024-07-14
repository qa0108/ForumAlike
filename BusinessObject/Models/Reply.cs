namespace DataAccess.Models
{
    public partial class Reply
    {
        public Reply()
        {
            InverseParentReply = new HashSet<Reply>();
        }

        public int ReplyId { get; set; }
        public int? PostId { get; set; }
        public int? UserId { get; set; }
        public int? ParentReplyId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual Reply? ParentReply { get; set; }
        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Reply> InverseParentReply { get; set; }
    }
}
