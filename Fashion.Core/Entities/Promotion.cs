using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class Promotion : BaseEntity
    {
        [Required]
        public int ItemId { get; set; }
        
        [Required]
        public PromotionType Type { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public decimal? DiscountPercentage { get; set; }
        
        public decimal? DiscountAmount { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        // Navigation properties
        public virtual Item Item { get; set; } = null!;
    }
} 