using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace GlamourHub.Models
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            Reviews = new HashSet<Review>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        //[StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
        public string Password { get; set; } = null!;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your first name.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}

