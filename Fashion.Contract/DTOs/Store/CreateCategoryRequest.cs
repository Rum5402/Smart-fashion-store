using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    public class CreateCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
    }
} 