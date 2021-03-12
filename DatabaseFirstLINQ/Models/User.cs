using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirstLINQ.Models
{
    public partial class User
    {
        public User()
        {
            ShoppingCarts = new HashSet<ShoppingCart>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
