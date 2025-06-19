using System;

namespace FimwiApi.Core.Models.DTOs
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DocumentType { get; set; } // Only allowed: "CC", "CE", "TI", "NIT"
        public string DocumentNumber { get; set; }
    }
} 