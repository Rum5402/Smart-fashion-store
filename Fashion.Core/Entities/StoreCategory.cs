using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    /// <summary>
    /// Store category entity for managing product categories with hierarchical structure
    /// </summary>
    public class StoreCategory : BaseEntity
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
        /// Whether the category is active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Parent category ID (for subcategories)
        /// </summary>
        public int? ParentCategoryId { get; set; }
        
        /// <summary>
        /// Display order for category sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
        /// <summary>
        /// Navigation property for parent category
        /// </summary>
        public virtual StoreCategory? ParentCategory { get; set; }
        
        /// <summary>
        /// Navigation property for sub-categories
        /// </summary>
        public virtual ICollection<StoreCategory> SubCategories { get; set; } = new List<StoreCategory>();
        
        /// <summary>
        /// Navigation property for items in this category
        /// </summary>
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public int StoreId { get; set; }
        public virtual StoreBrandSettings Store { get; set; } = null!;
    }
} 