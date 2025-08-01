namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Brand settings data transfer object for store configuration
    /// </summary>
    public class BrandSettingsDto
    {
        /// <summary>
        /// Unique identifier for the brand settings
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Store name (required field)
        /// </summary>
        public string StoreName { get; set; } = string.Empty;
        
        /// <summary>
        /// Store tagline (optional, max 40 characters)
        /// </summary>
        public string? Tagline { get; set; }
        
        /// <summary>
        /// Logo image URL (supports: JPG, PNG, WebP, Max 5MB each)
        /// </summary>
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// Primary brand color (hex format: #RRGGBB)
        /// </summary>
        public string? PrimaryColor { get; set; }
        
        /// <summary>
        /// Whether the brand settings are active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// When the brand settings were created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the brand settings were last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public string? StoreDescription { get; set; }
        public string? SecondaryColor { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? StoreAddress { get; set; }
        public string? LocationName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Floor { get; set; }
        public string? MallName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SecondaryPhoneNumber { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Website { get; set; }
        /// <summary>
        /// Social media links (as key-value pairs, e.g., facebook, instagram, twitter)
        /// </summary>
        public Dictionary<string, string>? SocialMedia { get; set; }
        public Dictionary<string, string>? SocialMediaLinks { get; set; }
    }
} 