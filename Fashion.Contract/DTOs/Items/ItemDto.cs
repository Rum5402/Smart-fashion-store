using Fashion.Core.Enums;

namespace Fashion.Contract.DTOs.Items
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public ItemCategory Category { get; set; }
        public ItemStyle Style { get; set; }
        public ProductType ProductType { get; set; }
        public StoreActivity StoreActivity { get; set; }
        public string? FabricType { get; set; }
        public string? SubCategory { get; set; }
        public string? BrandName { get; set; }
        public string? PrimaryColor { get; set; }
        public List<string> AvailableSizes { get; set; } = new();
        public List<string> AvailableColors { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        public bool IsNewCollection { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public CategoryDto? CategoryEntity { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
    
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
} 