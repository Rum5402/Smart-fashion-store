using System.ComponentModel.DataAnnotations;
using Fashion.Core.Enums;

namespace Fashion.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; } = UserRole.Visitor;
        
        [MaxLength(6)]
        public string? OTP { get; set; }
        
        public DateTime? OTPExpiry { get; set; }
        
        // Password for Customer accounts
        [MaxLength(255)]
        public string? PasswordHash { get; set; }
        
        // Guest/Customer specific data
        public int? Height { get; set; } // in cm
        public int? Weight { get; set; } // in kg
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? SkinTone { get; set; }
        
        public int StoreId { get; set; }
        public virtual StoreBrandSettings Store { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<FittingRoomRequest> FittingRoomRequests { get; set; } = new List<FittingRoomRequest>();
        public virtual ICollection<Item> SavedItems { get; set; } = new List<Item>();
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
} 