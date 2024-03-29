﻿using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlamourHub.Models
{
    [Authorize]
    public partial class Cart
    {
        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }
        public int Quantity { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
