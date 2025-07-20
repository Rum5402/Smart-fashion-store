using Fashion.Contract.DTOs.Items;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Response model for filtered products with pagination and metadata
    /// </summary>
    public class ProductFilterResponse
    {
        /// <summary>
        /// List of filtered products
        /// </summary>
        public List<ItemDto> Products { get; set; } = new();
        
        /// <summary>
        /// Total count of products matching the filter criteria
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; }
        
        /// <summary>
        /// Page size used
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Whether there are more pages available
        /// </summary>
        public bool HasNextPage { get; set; }
        
        /// <summary>
        /// Whether there are previous pages available
        /// </summary>
        public bool HasPreviousPage { get; set; }
        
        /// <summary>
        /// Filter metadata showing available options
        /// </summary>
        public FilterMetadata Metadata { get; set; } = new();
    }
    
    /// <summary>
    /// Metadata about available filter options
    /// </summary>
    public class FilterMetadata
    {
        /// <summary>
        /// Available categories with counts
        /// </summary>
        public List<FilterOption> Categories { get; set; } = new();
        
        /// <summary>
        /// Available styles with counts
        /// </summary>
        public List<FilterOption> Styles { get; set; } = new();
        
        /// <summary>
        /// Available product types with counts
        /// </summary>
        public List<FilterOption> ProductTypes { get; set; } = new();
        
        /// <summary>
        /// Available store activities with counts
        /// </summary>
        public List<FilterOption> StoreActivities { get; set; } = new();
        
        /// <summary>
        /// Available sizes with counts
        /// </summary>
        public List<FilterOption> Sizes { get; set; } = new();
        
        /// <summary>
        /// Available colors with counts
        /// </summary>
        public List<FilterOption> Colors { get; set; } = new();
        
        /// <summary>
        /// Available brand names with counts
        /// </summary>
        public List<FilterOption> BrandNames { get; set; } = new();
        
        /// <summary>
        /// Available fabric types with counts
        /// </summary>
        public List<FilterOption> FabricTypes { get; set; } = new();
        
        /// <summary>
        /// Price range information
        /// </summary>
        public PriceRange PriceRange { get; set; } = new();
    }
    
    /// <summary>
    /// Individual filter option with count
    /// </summary>
    public class FilterOption
    {
        /// <summary>
        /// Option value
        /// </summary>
        public string Value { get; set; } = string.Empty;
        
        /// <summary>
        /// Display name for the option
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
        
        /// <summary>
        /// Number of products matching this option
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// Whether this option is currently selected
        /// </summary>
        public bool IsSelected { get; set; }
    }
    
    /// <summary>
    /// Price range information
    /// </summary>
    public class PriceRange
    {
        /// <summary>
        /// Minimum price available
        /// </summary>
        public decimal MinPrice { get; set; }
        
        /// <summary>
        /// Maximum price available
        /// </summary>
        public decimal MaxPrice { get; set; }
        
        /// <summary>
        /// Average price
        /// </summary>
        public decimal AveragePrice { get; set; }
    }
} 