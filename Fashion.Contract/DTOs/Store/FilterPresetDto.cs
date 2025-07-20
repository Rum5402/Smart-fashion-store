using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// DTO for filter presets - predefined filter combinations
    /// </summary>
    public class FilterPresetDto
    {
        /// <summary>
        /// Preset ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Preset name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Preset description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Filter criteria for this preset
        /// </summary>
        public Dictionary<string, object> Filters { get; set; } = new();
        
        /// <summary>
        /// Preset icon or image URL
        /// </summary>
        public string? IconUrl { get; set; }
        
        /// <summary>
        /// Preset color theme
        /// </summary>
        public string? ColorTheme { get; set; }
        
        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Is preset active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Updated date
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Request model for creating a new filter preset
    /// </summary>
    public class CreateFilterPresetRequest
    {
        /// <summary>
        /// Preset name (required)
        /// </summary>
        [Required(ErrorMessage = "Preset name is required")]
        [MaxLength(100, ErrorMessage = "Preset name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Preset description (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Filter criteria for this preset
        /// </summary>
        [Required(ErrorMessage = "Filter criteria is required")]
        public ProductFilterRequest FilterCriteria { get; set; } = new();
        
        /// <summary>
        /// Whether this should be a default preset
        /// </summary>
        public bool IsDefault { get; set; } = false;
    }
    
    /// <summary>
    /// Request model for updating an existing filter preset
    /// </summary>
    public class UpdateFilterPresetRequest
    {
        /// <summary>
        /// Preset name (required)
        /// </summary>
        [Required(ErrorMessage = "Preset name is required")]
        [MaxLength(100, ErrorMessage = "Preset name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Preset description (optional)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Filter criteria for this preset
        /// </summary>
        [Required(ErrorMessage = "Filter criteria is required")]
        public ProductFilterRequest FilterCriteria { get; set; } = new();
        
        /// <summary>
        /// Whether this should be a default preset
        /// </summary>
        public bool IsDefault { get; set; } = false;
        
        /// <summary>
        /// Whether this preset is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 