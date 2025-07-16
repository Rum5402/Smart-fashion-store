using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class Item : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional product code (SKU or barcode)
        /// </summary>
        [MaxLength(100)]
        public string? ProductCode { get; set; }
        
        /// <summary>
        /// Optional product tags/keywords for search and filters (stored as JSON array)
        /// </summary>
        public string Tags { get; set; } = "[]";
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? OriginalPrice { get; set; }
        
        [Required]
        public ItemCategory Category { get; set; }
        
        [Required]
        public ItemStyle Style { get; set; }
        
        [Required]
        public ProductType ProductType { get; set; }
        
        [Required]
        public StoreActivity StoreActivity { get; set; }
        
        [MaxLength(100)]
        public string? FabricType { get; set; }
        
        [MaxLength(50)]
        public string? SubCategory { get; set; }
        
        [MaxLength(100)]
        public string? BrandName { get; set; }
        
        [MaxLength(50)]
        public string? PrimaryColor { get; set; } // Auto-detected color
        
        // Lists stored as JSON strings
        public string AvailableSizes { get; set; } = "[]"; // ["S", "M", "L", "XL"]
        public string AvailableColors { get; set; } = "[]"; // ["Red", "Blue", "Black"]
        public string ImageUrls { get; set; } = "[]"; // ["url1", "url2"]
        
        // Promotion flags
        public bool IsNewCollection { get; set; } = false;
        public bool IsBestSeller { get; set; } = false;
        public bool IsOnSale { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
        
        // Foreign keys
        /// <summary>
        /// Foreign key to Categories table (StoreCategoryId)
        /// </summary>
        public int? StoreCategoryId { get; set; }
        
        // Navigation properties
        public virtual Category? CategoryEntity { get; set; }
        public virtual ICollection<User> SavedByUsers { get; set; } = new List<User>();
        public virtual ICollection<FittingRoomRequest> FittingRoomRequests { get; set; } = new List<FittingRoomRequest>();
        public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
} 