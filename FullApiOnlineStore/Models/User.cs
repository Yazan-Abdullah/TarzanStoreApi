﻿using System;
using System.Collections.Generic;

namespace FullApiOnlineStore.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Logins = new HashSet<Login>();
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? UserTypeId { get; set; }

        public virtual UserType? UserType { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
