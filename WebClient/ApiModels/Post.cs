using System;
using System.Collections.Generic;

namespace WebClient.ApiModels
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

        public virtual Thread? Thread { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
