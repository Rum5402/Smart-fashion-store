using Fashion.Contract.DTOs.Items;
using Fashion.Contract.DTOs.Store;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;

namespace Fashion.Service.Store
{
    /// <summary>
    /// Service implementation for advanced product filtering functionality
    /// </summary>
    public class ProductFilterService : IProductFilterService
    {
        private readonly FashionDbContext _context;

        public ProductFilterService(FashionDbContext context)
        {
            _context = context;
        }

        public async Task<ProductFilterResponse> FilterProductsAsync(ProductFilterRequest request)
        {
            // Build the base query
            var query = _context.Items
                .Include(i => i.CategoryEntity)
                .Where(i => i.IsActive && !i.IsDeleted);

            // Apply filters
            query = ApplyFilters(query, request);

            // Apply sorting
            query = ApplySorting(query, request.SortOrder);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var skip = (request.Page - 1) * request.PageSize;
            var products = await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync();

            // Map to DTOs
            var productDtos = products.Select(MapToItemDto).ToList();

            // Calculate pagination info
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var hasNextPage = request.Page < totalPages;
            var hasPreviousPage = request.Page > 1;

            // Get metadata
            var metadata = await GetFilterMetadataAsync(request);

            return new ProductFilterResponse
            {
                Products = productDtos,
                TotalCount = totalCount,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                Metadata = metadata
            };
        }

        public async Task<FilterMetadata> GetFilterMetadataAsync(ProductFilterRequest? request = null)
        {
            var baseQuery = _context.Items.Where(i => i.IsActive && !i.IsDeleted);
            
            if (request != null)
            {
                baseQuery = ApplyFilters(baseQuery, request);
            }

            var metadata = new FilterMetadata();

            // Get categories
            metadata.Categories = await GetCategoryOptionsAsync(baseQuery, request);

            // Get styles
            metadata.Styles = await GetStyleOptionsAsync(baseQuery, request);

            // Get product types
            metadata.ProductTypes = await GetProductTypeOptionsAsync(baseQuery, request);

            // Get store activities
            metadata.StoreActivities = await GetStoreActivityOptionsAsync(baseQuery, request);

            // Get sizes
            metadata.Sizes = await GetSizeOptionsAsync(baseQuery, request);

            // Get colors
            metadata.Colors = await GetColorOptionsAsync(baseQuery, request);

            // Get brand names
            metadata.BrandNames = await GetBrandNameOptionsAsync(baseQuery, request);

            // Get fabric types
            metadata.FabricTypes = await GetFabricTypeOptionsAsync(baseQuery, request);

            // Get price range
            metadata.PriceRange = await GetPriceRangeAsync(request);

            return metadata;
        }

        public async Task<List<FilterOption>> GetFilterOptionsAsync(FilterType filterType, ProductFilterRequest? currentFilters = null)
        {
            var baseQuery = _context.Items.Where(i => i.IsActive && !i.IsDeleted);
            
            if (currentFilters != null)
            {
                baseQuery = ApplyFilters(baseQuery, currentFilters);
            }

            return filterType switch
            {
                FilterType.Size => await GetSizeOptionsAsync(baseQuery, currentFilters),
                FilterType.Color => await GetColorOptionsAsync(baseQuery, currentFilters),
                FilterType.Style => await GetStyleOptionsAsync(baseQuery, currentFilters),
                FilterType.Type => await GetProductTypeOptionsAsync(baseQuery, currentFilters),
                FilterType.Promotion => await GetPromotionOptionsAsync(baseQuery, currentFilters),
                _ => new List<FilterOption>()
            };
        }

        public async Task<PriceRange> GetPriceRangeAsync(ProductFilterRequest? request = null)
        {
            var baseQuery = _context.Items.Where(i => i.IsActive && !i.IsDeleted);
            
            if (request != null)
            {
                baseQuery = ApplyFilters(baseQuery, request);
            }

            var priceStats = await baseQuery
                .Select(i => i.Price)
                .ToListAsync();

            if (!priceStats.Any())
                return new PriceRange();

            return new PriceRange
            {
                MinPrice = priceStats.Min(),
                MaxPrice = priceStats.Max(),
                AveragePrice = priceStats.Average()
            };
        }

        #region Private Methods

        private IQueryable<Item> ApplyFilters(IQueryable<Item> query, ProductFilterRequest request)
        {
            // Search term
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(i => 
                    i.Name.ToLower().Contains(searchTerm) ||
                    (i.Description != null && i.Description.ToLower().Contains(searchTerm)) ||
                    (i.Tags != null && i.Tags.ToLower().Contains(searchTerm))
                );
            }

            // Categories
            if (request.Categories?.Any() == true)
            {
                query = query.Where(i => request.Categories.Contains(i.Category));
            }

            // Category IDs
            if (request.CategoryIds?.Any() == true)
            {
                query = query.Where(i => request.CategoryIds.Contains(i.StoreCategoryId ?? 0));
            }

            // Styles
            if (request.Styles?.Any() == true)
            {
                query = query.Where(i => request.Styles.Contains(i.Style));
            }

            // Product Types
            if (request.ProductTypes?.Any() == true)
            {
                query = query.Where(i => request.ProductTypes.Contains(i.ProductType));
            }

            // Store Activities
            if (request.StoreActivities?.Any() == true)
            {
                query = query.Where(i => request.StoreActivities.Contains(i.StoreActivity));
            }

            // Sizes
            if (request.Sizes?.Any() == true)
            {
                query = query.Where(i => 
                    i.AvailableSizes.Contains(request.Sizes.First())
                );
            }

            // Colors
            if (request.Colors?.Any() == true)
            {
                query = query.Where(i => 
                    i.AvailableColors.Contains(request.Colors.First())
                );
            }

            // Brand Names
            if (request.BrandNames?.Any() == true)
            {
                query = query.Where(i => i.BrandName != null && request.BrandNames.Contains(i.BrandName));
            }

            // Fabric Types
            if (request.FabricTypes?.Any() == true)
            {
                query = query.Where(i => i.FabricType != null && request.FabricTypes.Contains(i.FabricType));
            }

            // Price Range
            if (request.MinPrice.HasValue)
            {
                query = query.Where(i => i.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(i => i.Price <= request.MaxPrice.Value);
            }

            // Promotion Filters
            if (request.IsNewCollection.HasValue)
            {
                query = query.Where(i => i.IsNewCollection == request.IsNewCollection.Value);
            }

            if (request.IsBestSeller.HasValue)
            {
                query = query.Where(i => i.IsBestSeller == request.IsBestSeller.Value);
            }

            if (request.IsOnSale.HasValue)
            {
                query = query.Where(i => i.IsOnSale == request.IsOnSale.Value);
            }

            return query;
        }

        private IQueryable<Item> ApplySorting(IQueryable<Item> query, ProductSortOrder sortOrder)
        {
            return sortOrder switch
            {
                ProductSortOrder.Newest => query.OrderByDescending(i => i.CreatedAt),
                ProductSortOrder.Oldest => query.OrderBy(i => i.CreatedAt),
                ProductSortOrder.PriceLowToHigh => query.OrderBy(i => i.Price),
                ProductSortOrder.PriceHighToLow => query.OrderByDescending(i => i.Price),
                ProductSortOrder.NameAZ => query.OrderBy(i => i.Name),
                ProductSortOrder.NameZA => query.OrderByDescending(i => i.Name),
                ProductSortOrder.Popular => query.OrderByDescending(i => i.IsBestSeller).ThenByDescending(i => i.IsNewCollection),
                _ => query.OrderByDescending(i => i.CreatedAt)
            };
        }

        private async Task<List<FilterOption>> GetCategoryOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var categories = items
                .GroupBy(i => i.Category)
                .Select(g => new FilterOption
                {
                    Value = g.Key.ToString(),
                    DisplayName = g.Key.ToString(),
                    Count = g.Count(),
                    IsSelected = request?.Categories?.Contains(g.Key) == true
                })
                .OrderBy(c => c.DisplayName)
                .ToList();
            return categories;
        }

        private async Task<List<FilterOption>> GetStyleOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var styles = items
                .GroupBy(i => i.Style)
                .Select(g => new FilterOption
                {
                    Value = g.Key.ToString(),
                    DisplayName = g.Key.ToString(),
                    Count = g.Count(),
                    IsSelected = request?.Styles?.Contains(g.Key) == true
                })
                .OrderBy(s => s.DisplayName)
                .ToList();
            return styles;
        }

        private async Task<List<FilterOption>> GetProductTypeOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var productTypes = items
                .GroupBy(i => i.ProductType)
                .Select(g => new FilterOption
                {
                    Value = g.Key.ToString(),
                    DisplayName = g.Key.ToString(),
                    Count = g.Count(),
                    IsSelected = request?.ProductTypes?.Contains(g.Key) == true
                })
                .OrderBy(pt => pt.DisplayName)
                .ToList();
            return productTypes;
        }

        private async Task<List<FilterOption>> GetStoreActivityOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var activities = items
                .GroupBy(i => i.StoreActivity)
                .Select(g => new FilterOption
                {
                    Value = g.Key.ToString(),
                    DisplayName = g.Key.ToString(),
                    Count = g.Count(),
                    IsSelected = request?.StoreActivities?.Contains(g.Key) == true
                })
                .OrderBy(a => a.DisplayName)
                .ToList();
            return activities;
        }

        private async Task<List<FilterOption>> GetSizeOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var allSizes = items
                .SelectMany(i => JsonSerializer.Deserialize<List<string>>(i.AvailableSizes) ?? new List<string>())
                .Distinct()
                .ToList();

            var sizeOptions = allSizes
                .Select(size => new FilterOption
                {
                    Value = size,
                    DisplayName = size,
                    Count = items.Count(i => (JsonSerializer.Deserialize<List<string>>(i.AvailableSizes) ?? new List<string>()).Contains(size)),
                    IsSelected = request?.Sizes?.Contains(size) == true
                })
                .Where(option => option.Count > 0)
                .OrderBy(option => option.DisplayName)
                .ToList();

            return sizeOptions;
        }

        private async Task<List<FilterOption>> GetColorOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.ToListAsync();
            var allColors = items
                .SelectMany(i => JsonSerializer.Deserialize<List<string>>(i.AvailableColors) ?? new List<string>())
                .Distinct()
                .ToList();

            var colorOptions = allColors
                .Select(color => new FilterOption
                {
                    Value = color,
                    DisplayName = color,
                    Count = items.Count(i => (JsonSerializer.Deserialize<List<string>>(i.AvailableColors) ?? new List<string>()).Contains(color)),
                    IsSelected = request?.Colors?.Contains(color) == true
                })
                .Where(option => option.Count > 0)
                .OrderBy(option => option.DisplayName)
                .ToList();

            return colorOptions;
        }

        private async Task<List<FilterOption>> GetBrandNameOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.Where(i => !string.IsNullOrEmpty(i.BrandName)).ToListAsync();
            var brandNames = items
                .GroupBy(i => i.BrandName)
                .Select(g => new FilterOption
                {
                    Value = g.Key ?? "",
                    DisplayName = g.Key ?? "",
                    Count = g.Count(),
                    IsSelected = request?.BrandNames?.Contains(g.Key ?? "") == true
                })
                .OrderBy(b => b.DisplayName)
                .ToList();
            return brandNames;
        }

        private async Task<List<FilterOption>> GetFabricTypeOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var items = await baseQuery.Where(i => !string.IsNullOrEmpty(i.FabricType)).ToListAsync();
            var fabricTypes = items
                .GroupBy(i => i.FabricType)
                .Select(g => new FilterOption
                {
                    Value = g.Key ?? "",
                    DisplayName = g.Key ?? "",
                    Count = g.Count(),
                    IsSelected = request?.FabricTypes?.Contains(g.Key ?? "") == true
                })
                .OrderBy(f => f.DisplayName)
                .ToList();
            return fabricTypes;
        }

        private async Task<List<FilterOption>> GetPromotionOptionsAsync(IQueryable<Item> baseQuery, ProductFilterRequest? request)
        {
            var promotionOptions = new List<FilterOption>();

            // New Collection
            var newCollectionCount = await baseQuery.CountAsync(i => i.IsNewCollection);
            if (newCollectionCount > 0)
            {
                promotionOptions.Add(new FilterOption
                {
                    Value = "NewCollection",
                    DisplayName = "New Collection",
                    Count = newCollectionCount,
                    IsSelected = request?.IsNewCollection == true
                });
            }

            // Best Seller
            var bestSellerCount = await baseQuery.CountAsync(i => i.IsBestSeller);
            if (bestSellerCount > 0)
            {
                promotionOptions.Add(new FilterOption
                {
                    Value = "BestSeller",
                    DisplayName = "Best Seller",
                    Count = bestSellerCount,
                    IsSelected = request?.IsBestSeller == true
                });
            }

            // On Sale
            var onSaleCount = await baseQuery.CountAsync(i => i.IsOnSale);
            if (onSaleCount > 0)
            {
                promotionOptions.Add(new FilterOption
                {
                    Value = "OnSale",
                    DisplayName = "On Sale",
                    Count = onSaleCount,
                    IsSelected = request?.IsOnSale == true
                });
            }

            return promotionOptions;
        }

        private ItemDto MapToItemDto(Item item)
        {
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
                CategoryName = item.CategoryEntity?.Name,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
        }

        #endregion
    }
} 