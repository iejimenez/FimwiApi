using System;

namespace FimwiApi.Core.Models.DTOs
{
    public class BatchDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string BatchNumber { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 