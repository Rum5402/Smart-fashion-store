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

        [MaxLength(500, ErrorMessage = "Store description cannot exceed 500 characters")]
        public string? StoreDescription { get; set; }

        [MaxLength(7, ErrorMessage = "Secondary color must be a valid hex color code")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Secondary color must be a valid hex color code (e.g., #FF0000)")]
        public string? SecondaryColor { get; set; }

        [MaxLength(100, ErrorMessage = "Contact email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? ContactEmail { get; set; }

        [MaxLength(20, ErrorMessage = "Contact phone cannot exceed 20 characters")]
        public string? ContactPhone { get; set; }

        [MaxLength(200, ErrorMessage = "Store address cannot exceed 200 characters")]
        public string? StoreAddress { get; set; }

        [MaxLength(200)]
        public string? LocationName { get; set; }
        [MaxLength(200)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? Country { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [MaxLength(50)]
        public string? Floor { get; set; }
        [MaxLength(200)]
        public string? MallName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(20)]
        public string? SecondaryPhoneNumber { get; set; }
        [MaxLength(20)]
        public string? WhatsAppNumber { get; set; }
        [MaxLength(200)]
        public string? Website { get; set; }
        public Dictionary<string, string>? SocialMediaLinks { get; set; }
    }
} 