namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Data transfer object for store category information
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Unique identifier for the category
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Category name (displayed in UI)
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Category description (optional)
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Category image URL (optional)
        /// </summary>
        public string? ImageUrl { get; set; }
        
        /// <summary>
        /// Whether the category is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Parent category ID (for subcategories)
        /// </summary>
        public int? ParentCategoryId { get; set; }
        
        /// <summary>
        /// Parent category name (for display purposes)
        /// </summary>
        public string? ParentCategoryName { get; set; }
        
        /// <summary>
        /// Display order for category sorting
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// When the category was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the category was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Sub-categories (for hierarchical display)
        /// </summary>
        public List<CategoryDto> SubCategories { get; set; } = new();
        
        /// <summary>
        /// Number of items in this category
        /// </summary>
        public int ItemsCount { get; set; }
    }
} 