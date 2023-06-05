using System;
using System.Collections.Generic;

namespace FoodUsers.Infrastructure.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
