using Fashion.Contract.DTOs.Store;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            var settings = await _context.StoreBrandSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                return new StoreInfoDto();
            }
            Dictionary<string, string> socialMedia = new();
            if (!string.IsNullOrEmpty(settings.SocialMediaLinks))
            {
                try
                {
                    socialMedia = JsonSerializer.Deserialize<Dictionary<string, string>>(settings.SocialMediaLinks) ?? new();
                }
                catch { socialMedia = new(); }
            }
            return new StoreInfoDto
            {
                Id = settings.Id,
                BrandName = settings.StoreName ?? string.Empty,
                StoreName = settings.StoreName ?? string.Empty,
                LocationName = settings.LocationName ?? string.Empty,
                Address = settings.StoreAddress ?? string.Empty,
                PhoneNumber = settings.PhoneNumber ?? string.Empty,
                Email = settings.ContactEmail ?? string.Empty,
                Website = settings.Website ?? string.Empty,
                Description = settings.StoreDescription ?? string.Empty,
                LogoUrl = settings.LogoUrl ?? string.Empty,
                BannerUrl = string.Empty, // أضف إذا كان لديك حقل BannerUrl
                OpeningHours = string.Empty, // أضف إذا كان لديك حقل OpeningHours
                SocialMediaLinks = socialMedia,
                Features = new List<string>(), // أضف إذا كان لديك حقل Features
                IsActive = settings.IsActive,
                CreatedAt = settings.CreatedAt,
                UpdatedAt = settings.UpdatedAt
            };
        }

        public async Task<StoreLocationDto> GetStoreLocationAsync()
        {
            var settings = await _context.StoreBrandSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                return new StoreLocationDto();
            }
            return new StoreLocationDto
            {
                LocationName = settings.LocationName ?? "",
                Address = settings.StoreAddress ?? "",
                City = settings.City ?? "",
                Country = settings.Country ?? "",
                PostalCode = settings.PostalCode ?? "",
                Latitude = settings.Latitude,
                Longitude = settings.Longitude,
                Floor = settings.Floor ?? "",
                MallName = settings.MallName ?? ""
            };
        }

        public async Task<StoreContactDto> GetStoreContactAsync()
        {
            var settings = await _context.StoreBrandSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                return new StoreContactDto();
            }
            Dictionary<string, string>? socialMedia = null;
            if (!string.IsNullOrEmpty(settings.SocialMediaLinks))
            {
                try
                {
                    socialMedia = JsonSerializer.Deserialize<Dictionary<string, string>>(settings.SocialMediaLinks);
                }
                catch { socialMedia = null; }
            }
            return new StoreContactDto
            {
                PhoneNumber = settings.PhoneNumber ?? "",
                SecondaryPhoneNumber = settings.SecondaryPhoneNumber ?? "",
                Email = settings.ContactEmail ?? "",
                WhatsAppNumber = settings.WhatsAppNumber ?? "",
                Website = settings.Website ?? "",
                SocialMediaLinks = socialMedia
            };
        }

        public async Task<StoreDescriptionDto> GetStoreDescriptionAsync()
        {
            // Get the first store brand settings (assuming single store for now)
            var settings = await _context.StoreBrandSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Return default empty data if no settings exist
                return new StoreDescriptionDto
                {
                    Description = "",
                    AboutUs = "",
                    Mission = "",
                    Vision = "",
                    Values = new List<string>(),
                    Highlights = new List<string>()
                };
            }

            // Parse JSON arrays for Values and Highlights
            var values = new List<string>();
            var highlights = new List<string>();
            
            if (!string.IsNullOrEmpty(settings.Values))
            {
                try
                {
                    values = JsonSerializer.Deserialize<List<string>>(settings.Values) ?? new List<string>();
                }
                catch
                {
                    values = new List<string>();
                }
            }
            
            if (!string.IsNullOrEmpty(settings.Highlights))
            {
                try
                {
                    highlights = JsonSerializer.Deserialize<List<string>>(settings.Highlights) ?? new List<string>();
                }
                catch
                {
                    highlights = new List<string>();
                }
            }

            return new StoreDescriptionDto
            {
                Description = settings.StoreDescription ?? "",
                AboutUs = settings.AboutUs ?? "",
                Mission = settings.Mission ?? "",
                Vision = settings.Vision ?? "",
                Values = values,
                Highlights = highlights
            };
        }

        public async Task<StoreDescriptionDto> UpdateStoreDescriptionAsync(UpdateStoreDescriptionRequest request)
        {
            var settings = await _context.StoreBrandSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Create new settings if none exist
                settings = new StoreBrandSettings
                {
                    StoreName = "Default Store",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.StoreBrandSettings.Add(settings);
            }

            // Update the fields
            settings.StoreDescription = request.Description;
            settings.AboutUs = request.AboutUs;
            settings.Mission = request.Mission;
            settings.Vision = request.Vision;
            settings.Values = JsonSerializer.Serialize(request.Values ?? new List<string>());
            settings.Highlights = JsonSerializer.Serialize(request.Highlights ?? new List<string>());
            settings.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new StoreDescriptionDto
            {
                Description = settings.StoreDescription ?? "",
                AboutUs = settings.AboutUs ?? "",
                Mission = settings.Mission ?? "",
                Vision = settings.Vision ?? "",
                Values = request.Values ?? new List<string>(),
                Highlights = request.Highlights ?? new List<string>()
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
            settings.LocationName = request.LocationName;
            settings.City = request.City;
            settings.Country = request.Country;
            settings.PostalCode = request.PostalCode;
            settings.Latitude = request.Latitude;
            settings.Longitude = request.Longitude;
            settings.Floor = request.Floor;
            settings.MallName = request.MallName;
            settings.PhoneNumber = request.PhoneNumber;
            settings.SecondaryPhoneNumber = request.SecondaryPhoneNumber;
            settings.WhatsAppNumber = request.WhatsAppNumber;
            settings.Website = request.Website;
            settings.SocialMediaLinks = request.SocialMediaLinks != null ? JsonSerializer.Serialize(request.SocialMediaLinks) : null;
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