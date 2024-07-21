using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class FollowThread
    {
        public int UserId { get; set; }
        public int ThreadId { get; set; }
        public DateTime? FollowedAt { get; set; }

        public virtual Thread Thread { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
