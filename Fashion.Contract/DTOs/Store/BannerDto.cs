namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Data transfer object for store banner information
    /// </summary>
    public class BannerDto
    {
        /// <summary>
        /// Unique identifier for the banner
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Banner name (displayed in UI)
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Banner image URL (supports: JPG, PNG, WebP, Max 5MB each)
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Link URL for the banner (displayed as "Link: /collections/summer")
        /// </summary>
        public string? LinkUrl { get; set; }
        
        /// <summary>
        /// Whether the banner is active (displayed as "Active" or "inactive")
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Start date for banner display (optional)
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date for banner display (optional)
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// When the banner was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the banner was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Display order for sorting banners
        /// </summary>
        public int DisplayOrder { get; set; }
    }
} 