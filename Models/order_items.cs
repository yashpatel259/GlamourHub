using System.ComponentModel.DataAnnotations;

namespace GlamourHub.Models
{
    public class order_items
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to the Order table
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Foreign key to the Product table
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
