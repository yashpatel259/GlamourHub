﻿using System;
using System.Collections.Generic;

namespace GlamourHub.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
