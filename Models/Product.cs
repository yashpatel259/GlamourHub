using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlamourHub.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderItems = new HashSet<OrderItem>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [Column ("brand_id")]
        public int? BrandId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column ("image_path")]
        public string? ImagePath { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
