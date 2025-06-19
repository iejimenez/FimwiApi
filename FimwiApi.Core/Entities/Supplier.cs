using System;
using System.Collections.Generic;

namespace FimwiApi.Core.Entities
{
    public class Supplier : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DocumentType { get; set; } // Only allowed: "CC", "CE", "TI", "NIT"
        public string DocumentNumber { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
} 