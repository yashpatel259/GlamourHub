using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlamourHub.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
