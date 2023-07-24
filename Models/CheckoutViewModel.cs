using GlamourHub.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlamourHub.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; }

        [Required(ErrorMessage = "The First Name field is required.")]
        public string BillingFirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string BillingLastName { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string BillingStreet { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string BillingCity { get; set; }

        [Required(ErrorMessage = "The State/Province field is required.")]
        public string BillingState { get; set; }

        [Required(ErrorMessage = "The Postal Code field is required.")]
        public string BillingPostalCode { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public string BillingCountry { get; set; }

        // Shipping address properties with validation attributes
        [Required(ErrorMessage = "The First Name field is required.")]
        public string ShippingFirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string ShippingLastName { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string ShippingStreet { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string ShippingCity { get; set; }

        [Required(ErrorMessage = "The State/Province field is required.")]
        public string ShippingState { get; set; }

        [Required(ErrorMessage = "The Postal Code field is required.")]
        public string ShippingPostalCode { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public string ShippingCountry { get; set; }

        // Payment information properties with validation attributes
        [Required(ErrorMessage = "The Card Number field is required.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "The Expiration Date field is required.")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "The CVV field is required.")]
        public string Cvv { get; set; }

        // Billing address
        public Address BillingAddress { get; set; }

        // Shipping address
        public Address ShippingAddress { get; set; }
    }
}
