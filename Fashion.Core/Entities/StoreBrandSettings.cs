using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    /// <summary>
    /// Store brand settings entity for managing store branding configuration
    /// </summary>
    public class StoreBrandSettings : BaseEntity
    {
        /// <summary>
        /// Store name (required field)
        /// </summary>
        [Required(ErrorMessage = "Store name is required")]
        [MaxLength(200, ErrorMessage = "Store name cannot exceed 200 characters")]
        public string StoreName { get; set; } = string.Empty;
        
        /// <summary>
        /// Store tagline (optional, max 40 characters)
        /// </summary>
        [MaxLength(40, ErrorMessage = "Tagline cannot exceed 40 characters")]
        public string? Tagline { get; set; }
        
        /// <summary>
        /// Logo image URL (supports: JPG, PNG, WebP, Max 5MB each)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Logo URL cannot exceed 500 characters")]
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// Primary brand color (hex format: #RRGGBB)
        /// </summary>
        [MaxLength(7, ErrorMessage = "Primary color must be a valid hex color code")]
        public string? PrimaryColor { get; set; }
        
        /// <summary>
        /// Whether the brand settings are active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 