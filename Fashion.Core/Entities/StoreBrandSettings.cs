using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class StoreBrandSettings : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string StoreName { get; set; } = string.Empty;
        
        [MaxLength(40)]
        public string? Tagline { get; set; }
        
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
        
        [MaxLength(7)] // Hex color code
        public string? PrimaryColor { get; set; }
        
        [MaxLength(7)] // Hex color code
        public string? SecondaryColor { get; set; }
        
        [MaxLength(7)] // Hex color code
        public string? AccentColor { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Additional branding settings
        [MaxLength(500)]
        public string? AboutText { get; set; }
        
        [MaxLength(200)]
        public string? ContactEmail { get; set; }
        
        [MaxLength(20)]
        public string? ContactPhone { get; set; }
        
        [MaxLength(200)]
        public string? WebsiteUrl { get; set; }
        
        [MaxLength(500)]
        public string? SocialMediaLinks { get; set; } // JSON object for social media links
    }
} 