using Fashion.Contract.DTOs.Store;
using Fashion.Contract.DTOs.Items;
using Fashion.Service.Items;
using Fashion.Core.Enums;
using Fashion.Service.Common;

namespace Fashion.Service.Store
{
    /// <summary>
    /// Service implementation for Home page functionality
    /// </summary>
    public class HomeService : IHomeService
    {
        private readonly IItemService _itemService;
        private readonly IStoreControlService _storeControlService;
        private readonly ICacheService _cacheService;

        public HomeService(IItemService itemService, IStoreControlService storeControlService, ICacheService cacheService)
        {
            _itemService = itemService;
            _storeControlService = storeControlService;
            _cacheService = cacheService;
        }

        public async Task<HomePageDto> GetHomePageDataAsync(int storeId)
        {
            string cacheKey = $"home_page_data_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(15); // Cache for 15 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                // Get all required data in parallel for better performance
                var bannersTask = _storeControlService.GetStoreBannersAsync();
                var newCollectionTask = _itemService.GetNewCollectionAsync(storeId);
                var bestSellersTask = _itemService.GetBestSellersAsync(storeId);
                var onSaleTask = _itemService.GetOnSaleAsync(storeId);
                var categoriesTask = _storeControlService.GetStoreCategoriesAsync();
                var featuredProductsTask = _itemService.GetFeaturedProductsAsync(storeId);
                var storeInfoTask = _storeControlService.GetStoreInfoAsync();

                await Task.WhenAll(bannersTask, newCollectionTask, bestSellersTask, onSaleTask, categoriesTask, featuredProductsTask, storeInfoTask);

                return new HomePageDto
                {
                    Banners = await bannersTask,
                    NewCollection = await newCollectionTask,
                    BestSellers = await bestSellersTask,
                    OnSale = await onSaleTask,
                    Categories = await categoriesTask,
                    FeaturedProducts = await featuredProductsTask,
                    StoreInfo = await storeInfoTask
                };
            }, cacheExpiration);
        }

        public async Task<ShopByCategoryDto> GetShopByCategoryAsync(string category, int storeId)
        {
            var cacheKey = $"shop_category_{category}_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(10); // Cache for 10 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                var products = await _itemService.GetItemsByCategoryAsync(category, storeId);
                var filterOptions = await GetFilterOptionsForCategoryAsync(category, storeId);

                return new ShopByCategoryDto
                {
                    Category = category,
                    Products = products,
                    TotalCount = products.Count,
                    FilterOptions = filterOptions
                };
            }, cacheExpiration);
        }

        public async Task<CategoryFilterOptionsDto> GetFilterOptionsForCategoryAsync(string category, int storeId)
        {
            var cacheKey = $"filter_options_{category}_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(30); // Cache for 30 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                // Get all products for this category
                var categoryProducts = await _itemService.GetItemsByCategoryAsync(category, storeId);

                // Get product counts for different filter types
                var colorCounts = await _itemService.GetProductCountsByColorAsync();

                var filterOptions = new CategoryFilterOptionsDto();

                // Update product type counts
                filterOptions.ProductTypes.Options = new List<FilterOptionDto>
                {
                    new FilterOptionDto { Name = "T-Shirt", Count = categoryProducts.Count(p => p.ProductType == ProductType.TShirt) },
                    new FilterOptionDto { Name = "Sweat Pants", Count = categoryProducts.Count(p => p.ProductType == ProductType.Sweatpants) },
                    new FilterOptionDto { Name = "Pants", Count = categoryProducts.Count(p => p.ProductType == ProductType.Pants) },
                    new FilterOptionDto { Name = "Shirt", Count = categoryProducts.Count(p => p.ProductType == ProductType.Shirt) },
                    new FilterOptionDto { Name = "Shoes", Count = categoryProducts.Count(p => p.ProductType == ProductType.Shoes) }
                };

                // Update price range
                if (categoryProducts.Any())
                {
                    filterOptions.PriceRange.MinPrice = categoryProducts.Min(p => p.Price);
                    filterOptions.PriceRange.MaxPrice = categoryProducts.Max(p => p.Price);
                }

                // Update color counts
                filterOptions.Colors.Options = colorCounts.Select(kvp => new FilterOptionDto 
                { 
                    Name = kvp.Key, 
                    Count = kvp.Value 
                }).ToList();

                // Update style counts
                filterOptions.Styles.Options = new List<FilterOptionDto>
                {
                    new FilterOptionDto { Name = "Casual", Count = categoryProducts.Count(p => p.Style == ItemStyle.Casual) },
                    new FilterOptionDto { Name = "Formal", Count = categoryProducts.Count(p => p.Style == ItemStyle.Formal) },
                    new FilterOptionDto { Name = "Sport", Count = categoryProducts.Count(p => p.Style == ItemStyle.Sports) },
                    new FilterOptionDto { Name = "Outing", Count = categoryProducts.Count(p => p.Style == ItemStyle.Outing) }
                };

                // Update promotion counts
                filterOptions.Promotions.Options = new List<FilterOptionDto>
                {
                    new FilterOptionDto { Name = "New Collection", Count = categoryProducts.Count(p => p.IsNewCollection) },
                    new FilterOptionDto { Name = "Best Seller", Count = categoryProducts.Count(p => p.IsBestSeller) },
                    new FilterOptionDto { Name = "On Sale", Count = categoryProducts.Count(p => p.IsOnSale) }
                };

                return filterOptions;
            }, cacheExpiration);
        }

        public async Task<List<BannerDto>> GetBannersAsync()
        {
            const string cacheKey = "banners";
            var cacheExpiration = TimeSpan.FromMinutes(30); // Cache for 30 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _storeControlService.GetStoreBannersAsync();
            }, cacheExpiration);
        }

        public async Task<List<ItemDto>> GetNewCollectionAsync(int storeId)
        {
            string cacheKey = $"new_collection_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(20); // Cache for 20 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _itemService.GetNewCollectionAsync(storeId);
            }, cacheExpiration);
        }

        public async Task<List<ItemDto>> GetBestSellersAsync(int storeId)
        {
            string cacheKey = $"best_sellers_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(25); // Cache for 25 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _itemService.GetBestSellersAsync(storeId);
            }, cacheExpiration);
        }

        public async Task<List<ItemDto>> GetOnSaleAsync(int storeId)
        {
            string cacheKey = $"on_sale_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(15); // Cache for 15 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _itemService.GetOnSaleAsync(storeId);
            }, cacheExpiration);
        }

        public async Task<List<Fashion.Contract.DTOs.Store.CategoryDto>> GetCategoriesAsync()
        {
            const string cacheKey = "categories";
            var cacheExpiration = TimeSpan.FromMinutes(60); // Cache for 1 hour

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _storeControlService.GetStoreCategoriesAsync();
            }, cacheExpiration);
        }

        public async Task<List<ItemDto>> GetFeaturedProductsAsync(int storeId)
        {
            string cacheKey = $"featured_products_{storeId}";
            var cacheExpiration = TimeSpan.FromMinutes(20); // Cache for 20 minutes

            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _itemService.GetFeaturedProductsAsync(storeId);
            }, cacheExpiration);
        }


    }
} 