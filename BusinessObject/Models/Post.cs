using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Post
    {
        public Post()
        {
            Replies = new HashSet<Reply>();
        }

        public int PostId { get; set; }
        public int? ThreadId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string Title { get; set; } = null!;

        public virtual Thread? Thread { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
