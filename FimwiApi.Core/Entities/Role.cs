using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FimwiApi.Core.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<User> Users { get; set; }
    }
} 