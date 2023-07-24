using System;
using System.Collections.Generic;

namespace GlamourHub.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Billing address properties
        public string BillingFirstName { get; set; } = null!;
        public string BillingLastName { get; set; } = null!;    
        public string BillingStreet { get; set; } = null!;
        public string BillingCity { get; set; } = null!;
        public string BillingState { get; set; } = null!;
        public string BillingPostalCode { get; set; } = null!;
        // Add any other necessary properties for the billing address

        // Shipping address properties
        public  string ShippingFirstName { get; set; } = null!;
        public string ShippingLastName { get; set; } = null!;
        public string ShippingStreet { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingState { get; set; } = null!;
        public string ShippingPostalCode { get; set; } = null!;
        // Add any other necessary properties for the shipping address

        public virtual User? User { get; set; }
    }
}
