using System;

namespace FimwiApi.Core.Entities
{
    public class Batch : BaseEntity
    {
        public string BatchNumber { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public Product Product { get; set; }
    }
} 