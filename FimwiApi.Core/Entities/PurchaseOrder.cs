using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<PurchaseOrderItem> Items { get; set; }
    }
} 