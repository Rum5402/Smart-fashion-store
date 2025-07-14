using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class GuestLoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Range(100, 250)]
        public int? Height { get; set; } // in cm
        
        [Range(20, 200)]
        public int? Weight { get; set; } // in kg
        
        [Range(1, 120)]
        public int? Age { get; set; }
        
        public string? Gender { get; set; }
        
        public string? SkinTone { get; set; }
    }
} 