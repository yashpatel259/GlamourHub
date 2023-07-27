using System;
using System.Collections.Generic;

namespace GlamourHub.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public  string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual User? User { get; set; }
    }
}
