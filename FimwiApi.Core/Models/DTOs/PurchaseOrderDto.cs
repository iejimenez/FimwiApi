using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Models.DTOs
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public ICollection<PurchaseOrderItemDto> Items { get; set; }
    }
} 