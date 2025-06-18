using System.ComponentModel.DataAnnotations;

namespace FimwiApi.Core.Models.DTOs
{
    public class PermissionDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
} 