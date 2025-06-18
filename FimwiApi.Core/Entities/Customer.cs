using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; } // CC, TI, CE, NIT
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
} 