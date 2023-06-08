using System;
using System.Collections.Generic;

namespace GlamourHub.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string CardNumber { get; set; } = null!;
        public string ExpirationDate { get; set; } = null!;
        public string Cvv { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
