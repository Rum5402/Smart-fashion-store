using Fashion.Contract.DTOs.Items;

namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// Data transfer object for complete home page data
    /// </summary>
    public class HomePageDto
    {
        /// <summary>
        /// Store banners for promotional content
        /// </summary>
        public List<BannerDto> Banners { get; set; } = new List<BannerDto>();

        /// <summary>
        /// New collection products
        /// </summary>
        public List<ItemDto> NewCollection { get; set; } = new List<ItemDto>();

        /// <summary>
        /// Best seller products
        /// </summary>
        public List<ItemDto> BestSellers { get; set; } = new List<ItemDto>();

        /// <summary>
        /// On-sale products
        /// </summary>
        public List<ItemDto> OnSale { get; set; } = new List<ItemDto>();

        /// <summary>
        /// Store categories for shop by category
        /// </summary>
        public List<Store.CategoryDto> Categories { get; set; } = new List<Store.CategoryDto>();

        /// <summary>
        /// Featured products (combination of new collection, best sellers, and on sale)
        /// </summary>
        public List<ItemDto> FeaturedProducts { get; set; } = new List<ItemDto>();

        /// <summary>
        /// Store information
        /// </summary>
        public StoreInfoDto? StoreInfo { get; set; }
    }

    /// <summary>
    /// Data transfer object for shop by category page
    /// </summary>
    public class ShopByCategoryDto
    {
        /// <summary>
        /// Category name
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Products in this category
        /// </summary>
        public List<ItemDto> Products { get; set; } = new List<ItemDto>();

        /// <summary>
        /// Total count of products
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Filter options for this category
        /// </summary>
        public CategoryFilterOptionsDto FilterOptions { get; set; } = new CategoryFilterOptionsDto();
    }

    /// <summary>
    /// Data transfer object for category filter options
    /// </summary>
    public class CategoryFilterOptionsDto
    {
        /// <summary>
        /// Product type filter options
        /// </summary>
        public FilterOptionGroupDto ProductTypes { get; set; } = new FilterOptionGroupDto
        {
            Title = "By Product Type",
            Options = new List<FilterOptionDto>
            {
                new FilterOptionDto { Name = "T-Shirt", Count = 0 },
                new FilterOptionDto { Name = "Sweat Pants", Count = 0 },
                new FilterOptionDto { Name = "Pants", Count = 0 },
                new FilterOptionDto { Name = "Shirt", Count = 0 },
                new FilterOptionDto { Name = "Shoes", Count = 0 }
            }
        };

        /// <summary>
        /// Price range filter options
        /// </summary>
        public PriceRangeFilterDto PriceRange { get; set; } = new PriceRangeFilterDto
        {
            Title = "Price",
            MinPrice = 100,
            MaxPrice = 10000,
            CurrentMin = 100,
            CurrentMax = 10000,
            Currency = "EGP"
        };

        /// <summary>
        /// Color filter options
        /// </summary>
        public FilterOptionGroupDto Colors { get; set; } = new FilterOptionGroupDto
        {
            Title = "By Color",
            Options = new List<FilterOptionDto>()
        };

        /// <summary>
        /// Style filter options
        /// </summary>
        public FilterOptionGroupDto Styles { get; set; } = new FilterOptionGroupDto
        {
            Title = "By Style",
            Options = new List<FilterOptionDto>
            {
                new FilterOptionDto { Name = "Casual", Count = 0 },
                new FilterOptionDto { Name = "Formal", Count = 0 },
                new FilterOptionDto { Name = "Sport", Count = 0 },
                new FilterOptionDto { Name = "Outing", Count = 0 }
            }
        };

        /// <summary>
        /// Promotion filter options
        /// </summary>
        public FilterOptionGroupDto Promotions { get; set; } = new FilterOptionGroupDto
        {
            Title = "By Promotion",
            Options = new List<FilterOptionDto>
            {
                new FilterOptionDto { Name = "New Collection", Count = 0 },
                new FilterOptionDto { Name = "Best Seller", Count = 0 },
                new FilterOptionDto { Name = "On Sale", Count = 0 }
            }
        };
    }

    /// <summary>
    /// Data transfer object for filter option group
    /// </summary>
    public class FilterOptionGroupDto
    {
        /// <summary>
        /// Filter group title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Filter options in this group
        /// </summary>
        public List<FilterOptionDto> Options { get; set; } = new List<FilterOptionDto>();
    }

    /// <summary>
    /// Data transfer object for individual filter option
    /// </summary>
    public class FilterOptionDto
    {
        /// <summary>
        /// Filter option name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of products matching this filter
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Whether this filter option is selected
        /// </summary>
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Data transfer object for price range filter
    /// </summary>
    public class PriceRangeFilterDto
    {
        /// <summary>
        /// Filter title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Minimum price in the range
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price in the range
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Current minimum price selection
        /// </summary>
        public decimal CurrentMin { get; set; }

        /// <summary>
        /// Current maximum price selection
        /// </summary>
        public decimal CurrentMax { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; } = "EGP";
    }
} 