using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class ManagerProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? Position { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class UpdateManagerProfileRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [MaxLength(200)]
        public string? StoreName { get; set; }
        
        [MaxLength(500)]
        public string? StoreDescription { get; set; }
        
        [MaxLength(200)]
        public string? StoreAddress { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [MaxLength(100)]
        public string? Department { get; set; }
        
        [MaxLength(50)]
        public string? Position { get; set; }
        
        public string? ProfileImageUrl { get; set; }
    }

    public class ManagerProfileResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ManagerProfileDto? Profile { get; set; }
    }
} 