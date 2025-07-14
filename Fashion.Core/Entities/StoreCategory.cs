using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class StoreCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int? ParentCategoryId { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual StoreCategory? ParentCategory { get; set; }
        public virtual ICollection<StoreCategory> SubCategories { get; set; } = new List<StoreCategory>();
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
} 