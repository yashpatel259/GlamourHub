using System.ComponentModel.DataAnnotations;

namespace GlamourHub.Models
{
    public class OrderDetail
    {
        [Key] public int orderid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string productname { get; set; }
        public decimal productprice { get; set; }
        public int quantity { get; set; }
    }

}
