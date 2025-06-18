using System;

namespace FimwiApi.Core.Models.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public int CurrentStock { get; set; }
    }
} 