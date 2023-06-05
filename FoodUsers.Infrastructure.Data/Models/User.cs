using System;
using System.Collections.Generic;

namespace FoodUsers.Infrastructure.Data.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public int DNI { get; set; }
        public string Cellphone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public long RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
