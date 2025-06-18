using System.ComponentModel.DataAnnotations;

namespace FimwiApi.Core.Models.DTOs
{
    public class RevokeTokenDto
    {
        [Required]
        public string Token { get; set; }
    }
} 