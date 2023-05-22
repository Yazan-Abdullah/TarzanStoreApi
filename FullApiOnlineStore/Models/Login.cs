using System;
using System.Collections.Generic;

namespace FullApiOnlineStore.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public bool? IsActive { get; set; }
        public string? CurrentToken { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public virtual User? User { get; set; }
    }
}
