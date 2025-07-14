using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class ExploreModeRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
} 