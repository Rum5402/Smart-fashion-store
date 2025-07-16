using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Request model for updating an existing store banner
    /// </summary>
    public class UpdateBannerRequest
    {
        /// <summary>
        /// Banner name (required field)
        /// </summary>
        [Required(ErrorMessage = "Banner name is required")]
        [MaxLength(200, ErrorMessage = "Banner name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Banner description (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Banner image URL (supports: JPG, PNG, WebP, Max 5MB each)
        /// </summary>
        [Required(ErrorMessage = "Banner image is required")]
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [RegularExpression(@"^https?://.*\.(jpg|jpeg|png|webp)$", 
            ErrorMessage = "Image URL must be a valid image URL (JPG, PNG, WebP)")]
        public string ImageUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Link URL for the banner (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Link URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? LinkUrl { get; set; }
        
        /// <summary>
        /// Display order for banner sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
        /// <summary>
        /// Start date for banner display (optional)
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date for banner display (optional)
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Whether the banner is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 