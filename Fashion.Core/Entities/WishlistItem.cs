using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class WishlistItem : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
} 