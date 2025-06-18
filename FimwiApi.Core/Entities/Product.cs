using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public decimal CurrentStock { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Batch> Batches { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
} 