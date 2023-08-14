using System.ComponentModel.DataAnnotations;

namespace GlamourHub.Models
{
    public class ProductInventory
    {
        [Key] public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public int InStock { get; set; }
        public int SoldQuantity { get; set; }
    }
}
