namespace GlamourHub.Models
{
   public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalBill { get; set; }
    }
}
