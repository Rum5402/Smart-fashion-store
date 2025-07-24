using Fashion.Contract.DTOs.Store;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.Store
{
    public class StoreControlService : IStoreControlService
    {
        private readonly FashionDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StoreControlService(FashionDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        #region Categories
        public async Task<List<CategoryDto>> GetAllCategoriesAsync(int storeId)
        {
            var categories = await _context.StoreCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Items)
                .Where(c => !c.IsDeleted && c.StoreId == storeId)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
            return categories.Select(MapToCategoryDto).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id, int storeId)
        {
            var category = await _context.StoreCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.StoreId == storeId);
            return category != null ? MapToCategoryDto(category) : null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, int storeId)
        {
            var category = new StoreCategory
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                ParentCategoryId = request.ParentCategoryId,
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
                StoreId = storeId
            };
            _context.StoreCategories.Add(category);
            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(category.Id, storeId) ?? MapToCategoryDto(category);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryRequest request, int storeId)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.StoreId == storeId);
            if (category == null)
                return null;
            category.Name = request.Name;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;
            category.ParentCategoryId = request.ParentCategoryId;
            category.DisplayOrder = request.DisplayOrder;
            category.IsActive = request.IsActive;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(category.Id, storeId);
        }

        public async Task<bool> DeleteCategoryAsync(int id, int storeId)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.StoreId == storeId);
            if (category == null)
                return false;
            var hasSubCategories = await _context.StoreCategories
                .AnyAsync(c => c.ParentCategoryId == id && !c.IsDeleted && c.StoreId == storeId);
            if (hasSubCategories)
                return false;
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleCategoryStatusAsync(int id, int storeId)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.StoreId == storeId);
            if (category == null)
                return false;
            category.IsActive = !category.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Filters
        public async Task<List<FilterDto>> GetAllFiltersAsync(int storeId)
        {
            var filters = await _context.StoreFilters
                .Where(f => !f.IsDeleted && f.StoreId == storeId)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Name)
                .ToListAsync();
            return filters.Select(MapToFilterDto).ToList();
        }

        public async Task<FilterDto?> GetFilterByIdAsync(int id, int storeId)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted && f.StoreId == storeId);
            return filter != null ? MapToFilterDto(filter) : null;
        }

        public async Task<FilterDto> CreateFilterAsync(CreateFilterRequest request, int storeId)
        {
            var filter = new StoreFilter
            {
                Name = request.Name,
                Description = request.Description,
                Options = System.Text.Json.JsonSerializer.Serialize(request.Options),
                Type = request.Type,
                SelectionType = request.SelectionType,
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
                StoreId = storeId
            };
            _context.StoreFilters.Add(filter);
            await _context.SaveChangesAsync();
            return await GetFilterByIdAsync(filter.Id, storeId) ?? MapToFilterDto(filter);
        }

        public async Task<FilterDto?> UpdateFilterAsync(int id, UpdateFilterRequest request, int storeId)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted && f.StoreId == storeId);
            if (filter == null)
                return null;
            filter.Name = request.Name;
            filter.Description = request.Description;
            filter.Options = System.Text.Json.JsonSerializer.Serialize(request.Options);
            filter.Type = request.Type;
            filter.SelectionType = request.SelectionType;
            filter.DisplayOrder = request.DisplayOrder;
            filter.IsActive = request.IsActive;
            filter.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetFilterByIdAsync(filter.Id, storeId);
        }

        public async Task<bool> DeleteFilterAsync(int id, int storeId)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted && f.StoreId == storeId);
            if (filter == null)
                return false;
            filter.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleFilterStatusAsync(int id, int storeId)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted && f.StoreId == storeId);
            if (filter == null)
                return false;
            filter.IsActive = !filter.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Banners
        public async Task<List<BannerDto>> GetAllBannersAsync(int storeId)
        {
            var banners = await _context.StoreBanners
                .Where(b => !b.IsDeleted && b.StoreId == storeId)
                .OrderBy(b => b.DisplayOrder)
                .ThenBy(b => b.CreatedAt)
                .ToListAsync();
            return banners.Select(MapToBannerDto).ToList();
        }

        public async Task<BannerDto?> GetBannerByIdAsync(int id, int storeId)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted && b.StoreId == storeId);
            return banner != null ? MapToBannerDto(banner) : null;
        }

        public async Task<BannerDto> CreateBannerAsync(CreateBannerRequest request, int storeId)
        {
            var banner = new StoreBanner
            {
                Name = request.Name,
                ImageUrl = request.ImageUrl,
                LinkUrl = request.LinkUrl,
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
                StoreId = storeId
            };
            _context.StoreBanners.Add(banner);
            await _context.SaveChangesAsync();
            return await GetBannerByIdAsync(banner.Id, storeId) ?? MapToBannerDto(banner);
        }

        public async Task<BannerDto?> UpdateBannerAsync(int id, UpdateBannerRequest request, int storeId)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted && b.StoreId == storeId);
            if (banner == null)
                return null;
            banner.Name = request.Name;
            banner.ImageUrl = request.ImageUrl;
            banner.LinkUrl = request.LinkUrl;
            banner.DisplayOrder = request.DisplayOrder;
            banner.IsActive = request.IsActive;
            banner.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetBannerByIdAsync(banner.Id, storeId);
        }

        public async Task<bool> DeleteBannerAsync(int id, int storeId)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted && b.StoreId == storeId);
            if (banner == null)
                return false;
            banner.IsDeleted = true;
            banner.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleBannerStatusAsync(int id, int storeId)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted && b.StoreId == storeId);
            if (banner == null)
                return false;
            banner.IsActive = !banner.IsActive;
            banner.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Store Information
        public async Task<StoreInfoDto> GetStoreInfoAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            // For now, return hardcoded ZARA store information
            // In a real application, this would come from the database
            return new StoreInfoDto
            {
                Id = 1,
                BrandName = "ZARA",
                StoreName = "ZARA - Cairo Festival City",
                LocationName = "Cairo Festival City Mall (New Cairo)",
                Address = "Ring Road, Cairo Festival City, 2nd Floor, New Cairo, Cairo",
                PhoneNumber = "+2 01007972537",
                Email = "info@zara.com",
                Website = "https://www.zara.com",
                Description = "Step into a world of fashion that's always ahead. From timeless everyday wear to bold statement pieces, ZARA brings you the latest trends with a touch of effortless style. Redefine your wardrobe with pieces that speak your style, your way.",
                LogoUrl = "/images/zara-logo.png",
                BannerUrl = "/images/zara-banner.jpg",
                OpeningHours = "10:00 AM - 10:00 PM",
                SocialMediaLinks = new Dictionary<string, string>
                {
                    { "Facebook", "https://facebook.com/zara" },
                    { "Instagram", "https://instagram.com/zara" },
                    { "Twitter", "https://twitter.com/zara" }
                },
                Features = new List<string>
                {
                    "Fitting Rooms",
                    "Personal Styling",
                    "Alterations",
                    "Gift Cards",
                    "Loyalty Program"
                },
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public async Task<StoreLocationDto> GetStoreLocationAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            return new StoreLocationDto
            {
                LocationName = "Cairo Festival City Mall (New Cairo)",
                Address = "Ring Road, Cairo Festival City, 2nd Floor, New Cairo, Cairo",
                City = "New Cairo",
                Country = "Egypt",
                PostalCode = "11835",
                Latitude = 30.0444,
                Longitude = 31.2357,
                Floor = "2nd Floor",
                MallName = "Cairo Festival City Mall"
            };
        }

        public async Task<StoreContactDto> GetStoreContactAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            return new StoreContactDto
            {
                PhoneNumber = "+2 01007972537",
                SecondaryPhoneNumber = "+2 01007972538",
                Email = "cairo.festival@zara.com",
                WhatsAppNumber = "+2 01007972537",
                Website = "https://www.zara.com",
                SocialMediaLinks = new Dictionary<string, string>
                {
                    { "Facebook", "https://facebook.com/zara" },
                    { "Instagram", "https://instagram.com/zara" },
                    { "Twitter", "https://twitter.com/zara" },
                    { "YouTube", "https://youtube.com/zara" }
                }
            };
        }

        public async Task<StoreDescriptionDto> GetStoreDescriptionAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            return new StoreDescriptionDto
            {
                Description = "Step into a world of fashion that's always ahead. From timeless everyday wear to bold statement pieces, ZARA brings you the latest trends with a touch of effortless style. Redefine your wardrobe with pieces that speak your style, your way.",
                AboutUs = "ZARA is one of the world's largest international fashion companies. It belongs to Inditex, one of the world's largest distribution groups. The customer is at the heart of our unique business model, which includes design, production, distribution and sales through our extensive retail network.",
                Mission = "To provide high-quality, trendy fashion at affordable prices while maintaining sustainable practices and excellent customer service.",
                Vision = "To be the leading global fashion retailer, inspiring customers with innovative designs and exceptional shopping experiences.",
                Values = new List<string>
                {
                    "Innovation",
                    "Sustainability",
                    "Customer Focus",
                    "Quality",
                    "Diversity"
                },
                Highlights = new List<string>
                {
                    "Latest Fashion Trends",
                    "Sustainable Fashion",
                    "Personal Styling Services",
                    "Global Fashion Network",
                    "Fast Fashion Innovation"
                }
            };
        }

        public async Task<List<BannerDto>> GetStoreBannersAsync()
        {
            var banners = await _context.StoreBanners
                .Where(b => b.IsActive && !b.IsDeleted)
                .OrderBy(b => b.DisplayOrder)
                .ThenBy(b => b.CreatedAt)
                .ToListAsync();

            return banners.Select(MapToBannerDto).ToList();
        }

        public async Task<List<CategoryDto>> GetStoreCategoriesAsync()
        {
            var categories = await _context.StoreCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Where(c => c.IsActive && !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return categories.Select(MapToCategoryDto).ToList();
        }

        public async Task<List<FilterDto>> GetStoreFiltersAsync()
        {
            var filters = await _context.StoreFilters
                .Where(f => f.IsActive && !f.IsDeleted)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Name)
                .ToListAsync();

            return filters.Select(MapToFilterDto).ToList();
        }

        public async Task<List<FilterPresetDto>> GetStoreFilterPresetsAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            // For now, return hardcoded filter presets
            // In a real application, this would come from the database
            return new List<FilterPresetDto>
            {
                new FilterPresetDto
                {
                    Id = 1,
                    Name = "New Collection",
                    Description = "Latest arrivals and new collection items",
                    Filters = new Dictionary<string, object>
                    {
                        { "isNewCollection", true }
                    },
                    IsActive = true
                },
                new FilterPresetDto
                {
                    Id = 2,
                    Name = "Best Sellers",
                    Description = "Most popular and best-selling items",
                    Filters = new Dictionary<string, object>
                    {
                        { "isBestSeller", true }
                    },
                    IsActive = true
                },
                new FilterPresetDto
                {
                    Id = 3,
                    Name = "On Sale",
                    Description = "Items currently on sale with discounts",
                    Filters = new Dictionary<string, object>
                    {
                        { "isOnSale", true }
                    },
                    IsActive = true
                },
                new FilterPresetDto
                {
                    Id = 4,
                    Name = "Casual Wear",
                    Description = "Comfortable and casual clothing",
                    Filters = new Dictionary<string, object>
                    {
                        { "styles", new List<string> { "Casual" } }
                    },
                    IsActive = true
                },
                new FilterPresetDto
                {
                    Id = 5,
                    Name = "Formal Wear",
                    Description = "Professional and formal clothing",
                    Filters = new Dictionary<string, object>
                    {
                        { "styles", new List<string> { "Formal" } }
                    },
                    IsActive = true
                }
            };
        }
        #endregion

        #region Brand Settings
        public async Task<BrandSettingsDto?> GetBrandSettingsAsync()
        {
            var settings = await _context.StoreBrandSettings
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            return settings != null ? MapToBrandSettingsDto(settings) : null;
        }

        public async Task<BrandSettingsDto> UpdateBrandSettingsAsync(UpdateBrandSettingsRequest request)
        {
            var settings = await _context.StoreBrandSettings
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new StoreBrandSettings
                {
                    IsActive = true
                };
                _context.StoreBrandSettings.Add(settings);
            }

            settings.StoreName = request.StoreName;
            settings.Tagline = request.Tagline;
            settings.LogoUrl = request.LogoUrl;
            settings.PrimaryColor = request.PrimaryColor;
            settings.StoreDescription = request.StoreDescription;
            settings.SecondaryColor = request.SecondaryColor;
            settings.ContactEmail = request.ContactEmail;
            settings.ContactPhone = request.ContactPhone;
            settings.StoreAddress = request.StoreAddress;
            settings.SocialMedia = request.SocialMedia;
            settings.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToBrandSettingsDto(settings);
        }
        #endregion

        #region Mapping Methods
        private static CategoryDto MapToCategoryDto(StoreCategory category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                DisplayOrder = category.DisplayOrder,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                SubCategories = category.SubCategories.Select(MapToCategoryDto).ToList(),
                ItemsCount = category.Items.Count
            };
        }

        private static FilterDto MapToFilterDto(StoreFilter filter)
        {
            return new FilterDto
            {
                Id = filter.Id,
                Name = filter.Name,
                Description = filter.Description,
                Type = filter.Type,
                SelectionType = filter.SelectionType,
                Options = filter.GetOptions(),
                IsActive = filter.IsActive,
                DisplayOrder = filter.DisplayOrder,
                CreatedAt = filter.CreatedAt,
                UpdatedAt = filter.UpdatedAt
            };
        }

        private static BannerDto MapToBannerDto(StoreBanner banner)
        {
            return new BannerDto
            {
                Id = banner.Id,
                Name = banner.Name,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                IsActive = banner.IsActive,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
                DisplayOrder = banner.DisplayOrder,
                CreatedAt = banner.CreatedAt,
                UpdatedAt = banner.UpdatedAt
            };
        }

        private static BrandSettingsDto MapToBrandSettingsDto(StoreBrandSettings settings)
        {
            return new BrandSettingsDto
            {
                Id = settings.Id,
                StoreName = settings.StoreName,
                Tagline = settings.Tagline,
                LogoUrl = settings.LogoUrl,
                PrimaryColor = settings.PrimaryColor,
                IsActive = settings.IsActive,
                CreatedAt = settings.CreatedAt,
                UpdatedAt = settings.UpdatedAt,
                StoreDescription = settings.StoreDescription,
                SecondaryColor = settings.SecondaryColor,
                ContactEmail = settings.ContactEmail,
                ContactPhone = settings.ContactPhone,
                StoreAddress = settings.StoreAddress,
                SocialMedia = settings.SocialMedia
            };
        }
        #endregion
    }
} 