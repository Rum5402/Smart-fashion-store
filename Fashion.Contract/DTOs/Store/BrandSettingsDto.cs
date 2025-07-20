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
    }
} 