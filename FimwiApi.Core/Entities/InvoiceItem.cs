using System;

namespace FimwiApi.Core.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string Notes { get; set; }
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
} 