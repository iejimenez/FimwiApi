using System.ComponentModel.DataAnnotations;

namespace FimwiApi.Core.Models.DTOs
{
    public class RolePermissionDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid PermissionId { get; set; }
    }
} 