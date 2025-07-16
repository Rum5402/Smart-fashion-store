using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Request model for updating an existing store category
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <summary>
        /// Category name (required field)
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Category description (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Category image URL (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string? ImageUrl { get; set; }
        
        /// <summary>
        /// Parent category ID (for subcategories)
        /// </summary>
        public int? ParentCategoryId { get; set; }
        
        /// <summary>
        /// Display order for category sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
        /// <summary>
        /// Whether the category is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 