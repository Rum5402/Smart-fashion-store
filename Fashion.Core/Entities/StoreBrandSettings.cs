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

        /// <summary>
        /// Store description (optional, max 500 characters)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Store description cannot exceed 500 characters")]
        public string? StoreDescription { get; set; }

        /// <summary>
        /// Secondary brand color (hex format: #RRGGBB)
        /// </summary>
        [MaxLength(7, ErrorMessage = "Secondary color must be a valid hex color code")]
        public string? SecondaryColor { get; set; }

        /// <summary>
        /// Contact email for the store (optional)
        /// </summary>
        [MaxLength(100, ErrorMessage = "Contact email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contact phone for the store (optional)
        /// </summary>
        [MaxLength(20, ErrorMessage = "Contact phone cannot exceed 20 characters")]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Store address (optional, max 200 characters)
        /// </summary>
        [MaxLength(200, ErrorMessage = "Store address cannot exceed 200 characters")]
        public string? StoreAddress { get; set; }

        /// <summary>
        /// Social media links (as key-value pairs, e.g., facebook, instagram, twitter)
        /// </summary>
        public Dictionary<string, string>? SocialMedia { get; set; }

        /// <summary>
        /// About Us section (optional, max 2000 characters)
        /// </summary>
        [MaxLength(2000)]
        public string? AboutUs { get; set; }

        /// <summary>
        /// Mission statement (optional, max 1000 characters)
        /// </summary>
        [MaxLength(1000)]
        public string? Mission { get; set; }

        /// <summary>
        /// Vision statement (optional, max 1000 characters)
        /// </summary>
        [MaxLength(1000)]
        public string? Vision { get; set; }

        /// <summary>
        /// Values (as JSON array, e.g., ["Innovation", "Integrity", "Customer Focus"])
        /// </summary>
        public string? Values { get; set; }

        /// <summary>
        /// Highlights (as JSON array, e.g., ["Award Winner", "Fastest Growing Brand", "Best Customer Service"])
        /// </summary>
        public string? Highlights { get; set; }

        /// <summary>
        /// Date and time when the brand settings were created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the brand settings were last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 