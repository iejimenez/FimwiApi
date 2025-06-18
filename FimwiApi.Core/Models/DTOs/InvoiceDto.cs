using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Models.DTOs
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public ICollection<InvoiceItemDto> Items { get; set; }
    }
} 