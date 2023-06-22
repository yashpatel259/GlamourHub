using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlamourHub.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Brand name.")]
        public string Name { get; set; } = null!;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
