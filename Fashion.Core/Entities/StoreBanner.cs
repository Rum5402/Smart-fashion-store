using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    /// <summary>
    /// Store banner entity for managing promotional banners
    /// </summary>
    public class StoreBanner : BaseEntity
    {
        /// <summary>
        /// Banner name (required field)
        /// </summary>
        [Required(ErrorMessage = "Banner name is required")]
        [MaxLength(200, ErrorMessage = "Banner name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Banner image URL (supports: JPG, PNG, WebP, Max 5MB each)
        /// </summary>
        [Required(ErrorMessage = "Banner image is required")]
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string ImageUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Link URL for the banner (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Link URL cannot exceed 500 characters")]
        public string? LinkUrl { get; set; }
        
        /// <summary>
        /// Whether the banner is active (displayed as "Active" or "inactive")
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Start date for banner display (optional)
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date for banner display (optional)
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Display order for sorting banners
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
    }
} 