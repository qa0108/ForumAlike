﻿using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            FollowThreads = new HashSet<FollowThread>();
            Posts = new HashSet<Post>();
            Replies = new HashSet<Reply>();
            Threads = new HashSet<Thread>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<FollowThread> FollowThreads { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
    }
}
