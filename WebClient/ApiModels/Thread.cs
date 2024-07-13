using System;
using System.Collections.Generic;

namespace WebClient.ApiModels
{
    public partial class Thread
    {
        public Thread()
        {
            Posts = new HashSet<Post>();
        }

        public int ThreadId { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
