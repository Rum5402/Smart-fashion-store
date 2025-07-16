using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fashion.Core.Enums;

namespace Fashion.Core.Entities
{
    /// <summary>
    /// Store filter entity for managing product filters
    /// </summary>
    public class StoreFilter : BaseEntity
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
        /// Filter options stored as JSON array
        /// </summary>
        public string Options { get; set; } = "[]"; // JSON array of filter options
        
        /// <summary>
        /// Whether the filter is active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Display order for filter sorting
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
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
        /// Helper method to get filter options as list
        /// </summary>
        public List<string> GetOptions()
        {
            if (string.IsNullOrEmpty(Options))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(Options) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
        
        /// <summary>
        /// Helper method to set filter options from list
        /// </summary>
        public void SetOptions(List<string> options)
        {
            Options = JsonSerializer.Serialize(options);
        }
    }
} 