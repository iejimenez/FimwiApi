using System.ComponentModel.DataAnnotations;

namespace FimwiApi.Core.Models.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
} 