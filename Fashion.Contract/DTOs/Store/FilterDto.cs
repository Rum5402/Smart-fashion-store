using Fashion.Core.Enums;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Data transfer object for store filter information
    /// </summary>
    public class FilterDto
    {
        /// <summary>
        /// Unique identifier for the filter
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Filter name (displayed in UI)
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Filter description (optional)
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Filter type (Size, Type, Color, Style, Price, Promotion)
        /// </summary>
        public FilterType Type { get; set; }
        
        /// <summary>
        /// Selection type (Single or Multi)
        /// </summary>
        public SelectionType SelectionType { get; set; }
        
        /// <summary>
        /// Filter options (e.g., ["XS", "S", "M", "L", "XL", "XXL"] for Size)
        /// </summary>
        public List<string> Options { get; set; } = new();
        
        /// <summary>
        /// Whether the filter is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Display order for filter sorting
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// When the filter was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the filter was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
} 