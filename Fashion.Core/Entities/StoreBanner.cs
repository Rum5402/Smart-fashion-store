using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class StoreBanner : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? LinkUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
    }
} 