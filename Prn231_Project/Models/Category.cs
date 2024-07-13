using System;
using System.Collections.Generic;

namespace Prn231_Project.Models
{
    public partial class Category
    {
        public Category()
        {
            Threads = new HashSet<Thread>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
    }
}
