using Fashion.Contract.DTOs.Items;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using System.Text.Json;

namespace Fashion.Service.Items
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorDetectionService _colorDetectionService;

        public ItemService(IUnitOfWork unitOfWork, IColorDetectionService colorDetectionService)
        {
            _unitOfWork = unitOfWork;
            _colorDetectionService = colorDetectionService;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            return activeItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || !item.IsActive || item.IsDeleted)
                return null;
                
            var categories = await categoryRepository.GetAllAsync();
            return MapToDto(item, categories);
        }

        public async Task<List<ItemDto>> GetNewCollectionAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var newCollectionItems = items.Where(i => i.IsNewCollection && i.IsActive && !i.IsDeleted).ToList();
            
            return newCollectionItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetBestSellersAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var bestSellerItems = items.Where(i => i.IsBestSeller && i.IsActive && !i.IsDeleted).ToList();
            
            return bestSellerItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetOnSaleAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var onSaleItems = items.Where(i => i.IsOnSale && i.IsActive && !i.IsDeleted).ToList();
            
            return onSaleItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemRequest request)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Category = request.Category,
                Style = request.Style,
                ProductType = request.ProductType,
                StoreActivity = request.StoreActivity,
                FabricType = request.FabricType,
                SubCategory = request.SubCategory,
                BrandName = request.BrandName,
                ProductCode = request.ProductCode,
                Tags = JsonSerializer.Serialize(request.Tags ?? new List<string>()),
                AvailableSizes = JsonSerializer.Serialize(request.AvailableSizes),
                AvailableColors = JsonSerializer.Serialize(request.AvailableColors),
                ImageUrls = JsonSerializer.Serialize(request.ImageUrls),
                IsNewCollection = request.IsNewCollection,
                IsBestSeller = request.IsBestSeller,
                IsOnSale = request.IsOnSale,
                StoreCategoryId = request.StoreCategoryId,
                IsActive = true
            };

            // Auto-detect primary color if images are provided
            if (request.ImageUrls.Any())
            {
                item.PrimaryColor = await _colorDetectionService.DetectPrimaryColorAsync(request.ImageUrls.First());
            }

            await itemRepository.AddAsync(item);
            await _unitOfWork.SaveChangeAsync();

            return await GetItemByIdAsync(item.Id) ?? MapToDto(item, new List<StoreCategory>());
        }

        public async Task<ItemDto?> UpdateItemAsync(int id, UpdateItemRequest request)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return null;

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.OriginalPrice = request.OriginalPrice;
            item.Category = request.Category;
            item.Style = request.Style;
            item.ProductType = request.ProductType;
            item.StoreActivity = request.StoreActivity;
            item.FabricType = request.FabricType;
            item.SubCategory = request.SubCategory;
            item.BrandName = request.BrandName;
            item.ProductCode = request.ProductCode;
            item.Tags = JsonSerializer.Serialize(request.Tags ?? new List<string>());
            item.AvailableSizes = JsonSerializer.Serialize(request.AvailableSizes);
            item.AvailableColors = JsonSerializer.Serialize(request.AvailableColors);
            item.ImageUrls = JsonSerializer.Serialize(request.ImageUrls);
            item.IsNewCollection = request.IsNewCollection;
            item.IsBestSeller = request.IsBestSeller;
            item.IsOnSale = request.IsOnSale;
            item.IsActive = request.IsActive;
            item.StoreCategoryId = request.StoreCategoryId;
            item.UpdatedAt = DateTime.UtcNow;

            // Re-detect primary color if images changed
            if (request.ImageUrls.Any() && request.ImageUrls != JsonSerializer.Deserialize<List<string>>(item.ImageUrls))
            {
                item.PrimaryColor = await _colorDetectionService.DetectPrimaryColorAsync(request.ImageUrls.First());
            }

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();

            return await GetItemByIdAsync(item.Id);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return false;

            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> ToggleItemStatusAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return false;

            item.IsActive = !item.IsActive;
            item.UpdatedAt = DateTime.UtcNow;

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        // New methods for enhanced filtering
        public async Task<List<ItemDto>> GetItemsByCategoryAsync(string category)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var categoryItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                i.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return categoryItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetItemsByProductTypeAsync(string productType)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var productTypeItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                i.ProductType.ToString().Equals(productType, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return productTypeItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetItemsByStyleAsync(string style)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var styleItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                i.Style.ToString().Equals(style, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return styleItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetItemsByColorAsync(string color)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var colorItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                i.AvailableColors.Contains(color, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return colorItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var priceRangeItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                i.Price >= minPrice && i.Price <= maxPrice).ToList();
            
            return priceRangeItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<Dictionary<string, int>> GetProductCountsByCategoryAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var items = await itemRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            return activeItems
                .GroupBy(i => i.Category.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetProductCountsByProductTypeAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var items = await itemRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            return activeItems
                .GroupBy(i => i.ProductType.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetProductCountsByStyleAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var items = await itemRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            return activeItems
                .GroupBy(i => i.Style.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetProductCountsByColorAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var items = await itemRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            var colorCounts = new Dictionary<string, int>();
            
            foreach (var item in activeItems)
            {
                var colors = JsonSerializer.Deserialize<List<string>>(item.AvailableColors) ?? new();
                foreach (var color in colors)
                {
                    if (colorCounts.ContainsKey(color))
                        colorCounts[color]++;
                    else
                        colorCounts[color] = 1;
                }
            }
            
            return colorCounts;
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var items = await itemRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            if (!activeItems.Any())
                return new { minPrice = 0, maxPrice = 0, averagePrice = 0 };
            
            var prices = activeItems.Select(i => i.Price).ToList();
            
            return new 
            {
                minPrice = prices.Min(),
                maxPrice = prices.Max(),
                averagePrice = prices.Average()
            };
        }

        public async Task<List<ItemDto>> SearchProductsAsync(string query)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var searchItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                (i.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                 (i.Description != null && i.Description.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                 (i.Tags != null && i.Tags.Contains(query, StringComparison.OrdinalIgnoreCase)))).ToList();
            
            return searchItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetFeaturedProductsAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var featuredItems = items.Where(i => 
                i.IsActive && !i.IsDeleted && 
                (i.IsNewCollection || i.IsBestSeller || i.IsOnSale)).ToList();
            
            return featuredItems.Select(item => MapToDto(item, categories)).ToList();
        }

        // Mix & Match functionality
        public async Task<List<ItemDto>> GetMixMatchSuggestionsAsync(int baseItemId)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var baseItem = await itemRepository.GetByIdAsync(baseItemId);
            if (baseItem == null || !baseItem.IsActive || baseItem.IsDeleted)
                return new List<ItemDto>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            // Get items that complement the base item
            var suggestions = items.Where(i => 
                i.Id != baseItemId && 
                i.IsActive && !i.IsDeleted &&
                i.Category == baseItem.Category &&
                i.Style != baseItem.Style &&
                i.PrimaryColor != baseItem.PrimaryColor).ToList();
            
            return suggestions.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsAsync(string category)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var categoryItems = items.Where(i => 
                i.IsActive && !i.IsDeleted &&
                i.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            
            // Create outfit combinations (simplified logic)
            var outfits = new List<MixMatchCombinationDto>();
            var processedItems = new HashSet<int>();
            
            foreach (var item in categoryItems.Take(10)) // Limit to first 10 items
            {
                if (processedItems.Contains(item.Id)) continue;
                
                var complementaryItems = categoryItems.Where(i => 
                    i.Id != item.Id && 
                    i.Style != item.Style &&
                    !processedItems.Contains(i.Id)).Take(3).ToList();
                
                if (complementaryItems.Any())
                {
                    var outfitItems = new List<ItemDto> { MapToDto(item, categories) };
                    outfitItems.AddRange(complementaryItems.Select(i => MapToDto(i, categories)));
                    
                    outfits.Add(new MixMatchCombinationDto
                    {
                        Id = outfits.Count + 1,
                        Name = $"{category} Outfit {outfits.Count + 1}",
                        Description = $"A stylish {category.ToLower()} combination",
                        Items = outfitItems,
                        Category = category,
                        Style = item.Style.ToString(),
                        TotalPrice = outfitItems.Sum(i => i.Price),
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                    
                    processedItems.Add(item.Id);
                    foreach (var compItem in complementaryItems)
                    {
                        processedItems.Add(compItem.Id);
                    }
                }
            }
            
            return outfits;
        }

        public async Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsByStyleAsync(string style)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var styleItems = items.Where(i => 
                i.IsActive && !i.IsDeleted &&
                i.Style.ToString().Equals(style, StringComparison.OrdinalIgnoreCase)).ToList();
            
            // Create style-based outfit combinations
            var outfits = new List<MixMatchCombinationDto>();
            var processedItems = new HashSet<int>();
            
            foreach (var item in styleItems.Take(10))
            {
                if (processedItems.Contains(item.Id)) continue;
                
                var complementaryItems = styleItems.Where(i => 
                    i.Id != item.Id && 
                    i.Category != item.Category &&
                    !processedItems.Contains(i.Id)).Take(2).ToList();
                
                if (complementaryItems.Any())
                {
                    var outfitItems = new List<ItemDto> { MapToDto(item, categories) };
                    outfitItems.AddRange(complementaryItems.Select(i => MapToDto(i, categories)));
                    
                    outfits.Add(new MixMatchCombinationDto
                    {
                        Id = outfits.Count + 1,
                        Name = $"{style} Style Outfit {outfits.Count + 1}",
                        Description = $"A {style.ToLower()} style combination",
                        Items = outfitItems,
                        Category = item.Category.ToString(),
                        Style = style,
                        TotalPrice = outfitItems.Sum(i => i.Price),
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                    
                    processedItems.Add(item.Id);
                    foreach (var compItem in complementaryItems)
                    {
                        processedItems.Add(compItem.Id);
                    }
                }
            }
            
            return outfits;
        }

        public async Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsByOccasionAsync(string occasion)
        {
            // Map occasions to styles and categories
            var occasionMappings = new Dictionary<string, (string style, string category)>
            {
                { "Casual", ("Casual", "Men") },
                { "Formal", ("Formal", "Men") },
                { "Party", ("Casual", "Women") },
                { "Work", ("Formal", "Women") },
                { "Sport", ("Sport", "Men") }
            };
            
            if (!occasionMappings.ContainsKey(occasion))
                return new List<MixMatchCombinationDto>();
            
            var (style, category) = occasionMappings[occasion];
            
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var occasionItems = items.Where(i => 
                i.IsActive && !i.IsDeleted &&
                i.Style.ToString().Equals(style, StringComparison.OrdinalIgnoreCase) &&
                i.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            
            var outfits = new List<MixMatchCombinationDto>();
            var processedItems = new HashSet<int>();
            
            foreach (var item in occasionItems.Take(5))
            {
                if (processedItems.Contains(item.Id)) continue;
                
                var complementaryItems = occasionItems.Where(i => 
                    i.Id != item.Id && 
                    !processedItems.Contains(i.Id)).Take(2).ToList();
                
                if (complementaryItems.Any())
                {
                    var outfitItems = new List<ItemDto> { MapToDto(item, categories) };
                    outfitItems.AddRange(complementaryItems.Select(i => MapToDto(i, categories)));
                    
                    outfits.Add(new MixMatchCombinationDto
                    {
                        Id = outfits.Count + 1,
                        Name = $"{occasion} Outfit {outfits.Count + 1}",
                        Description = $"Perfect for {occasion.ToLower()} occasions",
                        Items = outfitItems,
                        Category = category,
                        Style = style,
                        Occasion = occasion,
                        TotalPrice = outfitItems.Sum(i => i.Price),
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                    
                    processedItems.Add(item.Id);
                    foreach (var compItem in complementaryItems)
                    {
                        processedItems.Add(compItem.Id);
                    }
                }
            }
            
            return outfits;
        }

        public async Task<List<MixMatchCombinationDto>> GetTrendingMixMatchAsync()
        {
            // For now, return featured products as trending combinations
            var featuredItems = await GetFeaturedProductsAsync();
            var combinations = new List<MixMatchCombinationDto>();
            
            for (int i = 0; i < featuredItems.Count - 2; i += 3)
            {
                var outfitItems = featuredItems.Skip(i).Take(3).ToList();
                if (outfitItems.Count == 3)
                {
                    combinations.Add(new MixMatchCombinationDto
                    {
                        Id = combinations.Count + 1,
                        Name = $"Trending Outfit {combinations.Count + 1}",
                        Description = "A trending fashion combination",
                        Items = outfitItems,
                        Category = outfitItems.First().Category.ToString(),
                        Style = outfitItems.First().Style.ToString(),
                        TotalPrice = outfitItems.Sum(i => i.Price),
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
            
            return combinations;
        }

        public async Task<List<MixMatchCombinationDto>> GetPersonalizedMixMatchRecommendationsAsync(string? userPreferences)
        {
            // For now, return trending combinations as personalized recommendations
            // In a real application, this would use user preferences and AI
            return await GetTrendingMixMatchAsync();
        }

        public async Task<MixMatchCombinationDto> SaveMixMatchCombinationAsync(SaveMixMatchRequest request)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<StoreCategory>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var combinationItems = items.Where(i => 
                request.ItemIds.Contains(i.Id) && 
                i.IsActive && !i.IsDeleted).ToList();
            
            var combination = new MixMatchCombinationDto
            {
                Id = new Random().Next(1000, 9999), // Generate random ID
                Name = request.Name,
                Description = request.Description,
                Items = combinationItems.Select(item => MapToDto(item, categories)).ToList(),
                Category = request.Category,
                Style = request.Style,
                Occasion = request.Occasion,
                Season = request.Season,
                Tags = request.Tags,
                TotalPrice = combinationItems.Sum(i => i.Price),
                IsPublic = request.IsPublic,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            // In a real application, this would be saved to the database
            return combination;
        }

        public async Task<List<MixMatchCombinationDto>> GetSavedMixMatchCombinationsAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            // For now, return empty list
            // In a real application, this would retrieve from database
            return new List<MixMatchCombinationDto>();
        }

        private static ItemDto MapToDto(Item item, IEnumerable<StoreCategory> categories)
        {
            var category = item.StoreCategoryId.HasValue 
                ? categories.FirstOrDefault(c => c.Id == item.StoreCategoryId.Value) 
                : null;
                
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                OriginalPrice = item.OriginalPrice,
                Category = item.Category,
                Style = item.Style,
                ProductType = item.ProductType,
                StoreActivity = item.StoreActivity,
                FabricType = item.FabricType,
                SubCategory = item.SubCategory,
                BrandName = item.BrandName,
                ProductCode = item.ProductCode,
                Tags = string.IsNullOrEmpty(item.Tags) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(item.Tags) ?? new(),
                PrimaryColor = item.PrimaryColor,
                AvailableSizes = JsonSerializer.Deserialize<List<string>>(item.AvailableSizes) ?? new(),
                AvailableColors = JsonSerializer.Deserialize<List<string>>(item.AvailableColors) ?? new(),
                ImageUrls = JsonSerializer.Deserialize<List<string>>(item.ImageUrls) ?? new(),
                IsNewCollection = item.IsNewCollection,
                IsBestSeller = item.IsBestSeller,
                IsOnSale = item.IsOnSale,
                IsActive = item.IsActive,
                StoreCategoryId = item.StoreCategoryId,
                CategoryName = category?.Name,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
        }
    }

    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<List<ItemDto>> GetNewCollectionAsync();
        Task<List<ItemDto>> GetBestSellersAsync();
        Task<List<ItemDto>> GetOnSaleAsync();
        Task<ItemDto> CreateItemAsync(CreateItemRequest request);
        Task<ItemDto?> UpdateItemAsync(int id, UpdateItemRequest request);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> ToggleItemStatusAsync(int id);
        
        // New methods for enhanced filtering
        Task<List<ItemDto>> GetItemsByCategoryAsync(string category);
        Task<List<ItemDto>> GetItemsByProductTypeAsync(string productType);
        Task<List<ItemDto>> GetItemsByStyleAsync(string style);
        Task<List<ItemDto>> GetItemsByColorAsync(string color);
        Task<List<ItemDto>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<Dictionary<string, int>> GetProductCountsByCategoryAsync();
        Task<Dictionary<string, int>> GetProductCountsByProductTypeAsync();
        Task<Dictionary<string, int>> GetProductCountsByStyleAsync();
        Task<Dictionary<string, int>> GetProductCountsByColorAsync();
        Task<object> GetPriceStatisticsAsync();
        Task<List<ItemDto>> SearchProductsAsync(string query);
        Task<List<ItemDto>> GetFeaturedProductsAsync();
        
        // Mix & Match functionality
        Task<List<ItemDto>> GetMixMatchSuggestionsAsync(int baseItemId);
        Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsAsync(string category);
        Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsByStyleAsync(string style);
        Task<List<MixMatchCombinationDto>> GetMixMatchOutfitsByOccasionAsync(string occasion);
        Task<List<MixMatchCombinationDto>> GetTrendingMixMatchAsync();
        Task<List<MixMatchCombinationDto>> GetPersonalizedMixMatchRecommendationsAsync(string? userPreferences);
        Task<MixMatchCombinationDto> SaveMixMatchCombinationAsync(SaveMixMatchRequest request);
        Task<List<MixMatchCombinationDto>> GetSavedMixMatchCombinationsAsync();
    }
} 