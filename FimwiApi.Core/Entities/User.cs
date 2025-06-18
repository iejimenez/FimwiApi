using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FimwiApi.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
} 