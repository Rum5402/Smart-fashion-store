using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    public class UpdateBrandSettingsRequest
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
        [RegularExpression(@"^https?://.*\.(jpg|jpeg|png|webp)$", 
            ErrorMessage = "Logo URL must be a valid image URL (JPG, PNG, WebP)")]
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// Primary brand color (hex format: #RRGGBB)
        /// </summary>
        [MaxLength(7, ErrorMessage = "Primary color must be a valid hex color code")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", 
            ErrorMessage = "Primary color must be a valid hex color code (e.g., #FF0000)")]
        public string? PrimaryColor { get; set; }
        
        /// <summary>
        /// Secondary brand color (hex format: #RRGGBB)
        /// </summary>
        [MaxLength(7, ErrorMessage = "Secondary color must be a valid hex color code")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", 
            ErrorMessage = "Secondary color must be a valid hex color code (e.g., #FF0000)")]
        public string? SecondaryColor { get; set; }
        
        /// <summary>
        /// Accent brand color (hex format: #RRGGBB)
        /// </summary>
        [MaxLength(7, ErrorMessage = "Accent color must be a valid hex color code")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", 
            ErrorMessage = "Accent color must be a valid hex color code (e.g., #FF0000)")]
        public string? AccentColor { get; set; }
        
        /// <summary>
        /// About text for the store
        /// </summary>
        [MaxLength(500, ErrorMessage = "About text cannot exceed 500 characters")]
        public string? AboutText { get; set; }
        
        /// <summary>
        /// Contact email address
        /// </summary>
        [MaxLength(200, ErrorMessage = "Contact email cannot exceed 200 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? ContactEmail { get; set; }
        
        /// <summary>
        /// Contact phone number
        /// </summary>
        [MaxLength(20, ErrorMessage = "Contact phone cannot exceed 20 characters")]
        public string? ContactPhone { get; set; }
        
        /// <summary>
        /// Store website URL
        /// </summary>
        [MaxLength(200, ErrorMessage = "Website URL cannot exceed 200 characters")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? WebsiteUrl { get; set; }
        
        /// <summary>
        /// Social media links (JSON format)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Social media links cannot exceed 500 characters")]
        public string? SocialMediaLinks { get; set; }
    }
} 