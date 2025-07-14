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
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.StoreCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Items)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return categories.Select(MapToCategoryDto).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.StoreCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            return category != null ? MapToCategoryDto(category) : null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var category = new StoreCategory
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                ParentCategoryId = request.ParentCategoryId,
                DisplayOrder = request.DisplayOrder,
                IsActive = true
            };

            _context.StoreCategories.Add(category);
            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id) ?? MapToCategoryDto(category);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

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

            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null)
                return false;

            // Check if category has subcategories
            var hasSubCategories = await _context.StoreCategories
                .AnyAsync(c => c.ParentCategoryId == id && !c.IsDeleted);

            if (hasSubCategories)
                return false; // Cannot delete category with subcategories

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleCategoryStatusAsync(int id)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null)
                return false;

            category.IsActive = !category.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Filters
        public async Task<List<FilterDto>> GetAllFiltersAsync()
        {
            var filters = await _context.StoreFilters
                .Where(f => !f.IsDeleted)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Name)
                .ToListAsync();

            return filters.Select(MapToFilterDto).ToList();
        }

        public async Task<FilterDto?> GetFilterByIdAsync(int id)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            return filter != null ? MapToFilterDto(filter) : null;
        }

        public async Task<FilterDto> CreateFilterAsync(CreateFilterRequest request)
        {
            var filter = new StoreFilter
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                SelectionType = request.SelectionType,
                DisplayOrder = request.DisplayOrder,
                IsActive = true
            };

            filter.SetOptions(request.Options);

            _context.StoreFilters.Add(filter);
            await _context.SaveChangesAsync();

            return MapToFilterDto(filter);
        }

        public async Task<FilterDto?> UpdateFilterAsync(int id, UpdateFilterRequest request)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (filter == null)
                return null;

            filter.Name = request.Name;
            filter.Description = request.Description;
            filter.Type = request.Type;
            filter.SelectionType = request.SelectionType;
            filter.DisplayOrder = request.DisplayOrder;
            filter.IsActive = request.IsActive;
            filter.UpdatedAt = DateTime.UtcNow;

            filter.SetOptions(request.Options);

            await _context.SaveChangesAsync();

            return MapToFilterDto(filter);
        }

        public async Task<bool> DeleteFilterAsync(int id)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (filter == null)
                return false;

            filter.IsDeleted = true;
            filter.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleFilterStatusAsync(int id)
        {
            var filter = await _context.StoreFilters
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (filter == null)
                return false;

            filter.IsActive = !filter.IsActive;
            filter.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Banners
        public async Task<List<BannerDto>> GetAllBannersAsync()
        {
            var banners = await _context.StoreBanners
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.DisplayOrder)
                .ThenBy(b => b.Name)
                .ToListAsync();

            return banners.Select(MapToBannerDto).ToList();
        }

        public async Task<BannerDto?> GetBannerByIdAsync(int id)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            return banner != null ? MapToBannerDto(banner) : null;
        }

        public async Task<BannerDto> CreateBannerAsync(CreateBannerRequest request)
        {
            var banner = new StoreBanner
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                LinkUrl = request.LinkUrl,
                DisplayOrder = request.DisplayOrder,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            _context.StoreBanners.Add(banner);
            await _context.SaveChangesAsync();

            return MapToBannerDto(banner);
        }

        public async Task<BannerDto?> UpdateBannerAsync(int id, UpdateBannerRequest request)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (banner == null)
                return null;

            banner.Name = request.Name;
            banner.Description = request.Description;
            banner.ImageUrl = request.ImageUrl;
            banner.LinkUrl = request.LinkUrl;
            banner.DisplayOrder = request.DisplayOrder;
            banner.StartDate = request.StartDate;
            banner.EndDate = request.EndDate;
            banner.IsActive = request.IsActive;
            banner.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToBannerDto(banner);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (banner == null)
                return false;

            banner.IsDeleted = true;
            banner.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleBannerStatusAsync(int id)
        {
            var banner = await _context.StoreBanners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (banner == null)
                return false;

            banner.IsActive = !banner.IsActive;
            banner.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
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
            settings.SecondaryColor = request.SecondaryColor;
            settings.AccentColor = request.AccentColor;
            settings.AboutText = request.AboutText;
            settings.ContactEmail = request.ContactEmail;
            settings.ContactPhone = request.ContactPhone;
            settings.WebsiteUrl = request.WebsiteUrl;
            settings.SocialMediaLinks = request.SocialMediaLinks;
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
                Description = banner.Description,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                IsActive = banner.IsActive,
                DisplayOrder = banner.DisplayOrder,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
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
                SecondaryColor = settings.SecondaryColor,
                AccentColor = settings.AccentColor,
                IsActive = settings.IsActive,
                AboutText = settings.AboutText,
                ContactEmail = settings.ContactEmail,
                ContactPhone = settings.ContactPhone,
                WebsiteUrl = settings.WebsiteUrl,
                SocialMediaLinks = settings.SocialMediaLinks,
                CreatedAt = settings.CreatedAt,
                UpdatedAt = settings.UpdatedAt
            };
        }
        #endregion
    }
} 