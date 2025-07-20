using Fashion.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Request model for filtering products with multiple criteria
    /// </summary>
    public class ProductFilterRequest
    {
        /// <summary>
        /// Search term for product name, description, or tags
        /// </summary>
        public string? SearchTerm { get; set; }
        
        /// <summary>
        /// Category IDs to filter by
        /// </summary>
        public List<int>? CategoryIds { get; set; }
        
        /// <summary>
        /// Product categories (Men, Women, Kids)
        /// </summary>
        public List<ItemCategory>? Categories { get; set; }
        
        /// <summary>
        /// Product styles to filter by
        /// </summary>
        public List<ItemStyle>? Styles { get; set; }
        
        /// <summary>
        /// Product types to filter by
        /// </summary>
        public List<ProductType>? ProductTypes { get; set; }
        
        /// <summary>
        /// Store activities to filter by
        /// </summary>
        public List<StoreActivity>? StoreActivities { get; set; }
        
        /// <summary>
        /// Sizes to filter by
        /// </summary>
        public List<string>? Sizes { get; set; }
        
        /// <summary>
        /// Colors to filter by
        /// </summary>
        public List<string>? Colors { get; set; }
        
        /// <summary>
        /// Brand names to filter by
        /// </summary>
        public List<string>? BrandNames { get; set; }
        
        /// <summary>
        /// Fabric types to filter by
        /// </summary>
        public List<string>? FabricTypes { get; set; }
        
        /// <summary>
        /// Minimum price for filtering
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be non-negative")]
        public decimal? MinPrice { get; set; }
        
        /// <summary>
        /// Maximum price for filtering
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be non-negative")]
        public decimal? MaxPrice { get; set; }
        
        /// <summary>
        /// Filter by new collection items
        /// </summary>
        public bool? IsNewCollection { get; set; }
        
        /// <summary>
        /// Filter by best seller items
        /// </summary>
        public bool? IsBestSeller { get; set; }
        
        /// <summary>
        /// Filter by on sale items
        /// </summary>
        public bool? IsOnSale { get; set; }
        
        /// <summary>
        /// Sort order for results
        /// </summary>
        public ProductSortOrder SortOrder { get; set; } = ProductSortOrder.Newest;
        
        /// <summary>
        /// Page number for pagination (1-based)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// Page size for pagination
        /// </summary>
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 20;
    }
    
    /// <summary>
    /// Available sort orders for product filtering
    /// </summary>
    public enum ProductSortOrder
    {
        Newest = 1,
        Oldest = 2,
        PriceLowToHigh = 3,
        PriceHighToLow = 4,
        NameAZ = 5,
        NameZA = 6,
        Popular = 7
    }
} 