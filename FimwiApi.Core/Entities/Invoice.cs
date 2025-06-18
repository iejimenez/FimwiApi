using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<InvoiceItem> Items { get; set; }
    }
} 