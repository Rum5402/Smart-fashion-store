using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Request model for updating an existing store filter
    /// </summary>
    public class UpdateFilterRequest
    {
        /// <summary>
        /// Filter name (required field)
        /// </summary>
        [Required(ErrorMessage = "Filter name is required")]
        [MaxLength(100, ErrorMessage = "Filter name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Filter description (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Filter type (Size, Type, Color, Style, Price, Promotion)
        /// </summary>
        [Required(ErrorMessage = "Filter type is required")]
        public FilterType Type { get; set; }
        
        /// <summary>
        /// Selection type (Single or Multi)
        /// </summary>
        [Required(ErrorMessage = "Selection type is required")]
        public SelectionType SelectionType { get; set; }
        
        /// <summary>
        /// Filter options (e.g., ["XS", "S", "M", "L", "XL", "XXL"] for Size)
        /// </summary>
        [Required(ErrorMessage = "Filter options are required")]
        [MinLength(1, ErrorMessage = "At least one option is required")]
        public List<string> Options { get; set; } = new();
        
        /// <summary>
        /// Display order for filter sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
        /// <summary>
        /// Whether the filter is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 