using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    public class UpdateFilterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public FilterType Type { get; set; }
        
        [Required]
        public SelectionType SelectionType { get; set; }
        
        public List<string> Options { get; set; } = new();
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
    }
} 