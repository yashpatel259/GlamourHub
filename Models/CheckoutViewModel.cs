using GlamourHub.Models;
using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlamourHub.ViewModels
{

    public class CheckoutViewModel
    {
        public List<Cart>? CartItems { get; set; }

        // Shipping address properties with validation attributes
        [Required(ErrorMessage = "The First Name field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The State/Province field is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "The Postal Code field is required.")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Invalid Canadian Postal Code.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "The Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Phone number. Please enter a 10-digit number without any spaces or dashes.")]
        public string Phone { get; set; }

        // Payment information properties with validation attributes
        [Required(ErrorMessage = "The Card Number field is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid Card Number. Please enter a 16-digit number.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "The Expiration Date field is required.")]
        [CustomValidation(typeof(CheckoutViewModel), "ValidateExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "The CVV field is required.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVV. Please enter a 3-digit number.")]
        public string Cvv { get; set; }

        // Order summary properties
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DeliveryAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsFreeShipping { get; set; }

        // Custom validation method for validating expiration date not in the past
        public static ValidationResult ValidateExpirationDate(DateTime date, ValidationContext context)
        {
            if (date < DateTime.Now)
            {
                return new ValidationResult("Expiration Date should not be in the past.");
            }
            return ValidationResult.Success;
        }
    }

}
