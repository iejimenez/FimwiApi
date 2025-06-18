using System;

namespace FimwiApi.Core.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string Notes { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Product Product { get; set; }
    }
} 