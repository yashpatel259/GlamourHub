using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace GlamourHub.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to the User table
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DeliveryAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsFreeShipping { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation property for the order items
        public List<order_items> order_items { get; set; }
    }
}
