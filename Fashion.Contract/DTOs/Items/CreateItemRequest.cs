using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Items
{
    public class CreateItemRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(0.01, double.MaxValue)]
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
        
        public List<string> AvailableSizes { get; set; } = new();
        public List<string> AvailableColors { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        
        public bool IsNewCollection { get; set; } = false;
        public bool IsBestSeller { get; set; } = false;
        public bool IsOnSale { get; set; } = false;
        
        public int? CategoryId { get; set; }
    }
} 