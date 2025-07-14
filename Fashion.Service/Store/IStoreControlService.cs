using Fashion.Contract.DTOs.Store;

namespace Fashion.Service.Store
{
    public interface IStoreControlService
    {
        // Categories
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> ToggleCategoryStatusAsync(int id);
        
        // Filters
        Task<List<FilterDto>> GetAllFiltersAsync();
        Task<FilterDto?> GetFilterByIdAsync(int id);
        Task<FilterDto> CreateFilterAsync(CreateFilterRequest request);
        Task<FilterDto?> UpdateFilterAsync(int id, UpdateFilterRequest request);
        Task<bool> DeleteFilterAsync(int id);
        Task<bool> ToggleFilterStatusAsync(int id);
        
        // Banners
        Task<List<BannerDto>> GetAllBannersAsync();
        Task<BannerDto?> GetBannerByIdAsync(int id);
        Task<BannerDto> CreateBannerAsync(CreateBannerRequest request);
        Task<BannerDto?> UpdateBannerAsync(int id, UpdateBannerRequest request);
        Task<bool> DeleteBannerAsync(int id);
        Task<bool> ToggleBannerStatusAsync(int id);
        
        // Brand Settings
        Task<BrandSettingsDto?> GetBrandSettingsAsync();
        Task<BrandSettingsDto> UpdateBrandSettingsAsync(UpdateBrandSettingsRequest request);
    }
} 