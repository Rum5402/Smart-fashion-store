using Fashion.Contract.DTOs.Store;

namespace Fashion.Service.Store
{
    public interface IStoreControlService
    {
        // Categories
        Task<List<CategoryDto>> GetAllCategoriesAsync(int storeId);
        Task<CategoryDto?> GetCategoryByIdAsync(int id, int storeId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, int storeId);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryRequest request, int storeId);
        Task<bool> DeleteCategoryAsync(int id, int storeId);
        Task<bool> ToggleCategoryStatusAsync(int id, int storeId);
        
        // Filters
        Task<List<FilterDto>> GetAllFiltersAsync(int storeId);
        Task<FilterDto?> GetFilterByIdAsync(int id, int storeId);
        Task<FilterDto> CreateFilterAsync(CreateFilterRequest request, int storeId);
        Task<FilterDto?> UpdateFilterAsync(int id, UpdateFilterRequest request, int storeId);
        Task<bool> DeleteFilterAsync(int id, int storeId);
        Task<bool> ToggleFilterStatusAsync(int id, int storeId);
        
        // Banners
        Task<List<BannerDto>> GetAllBannersAsync(int storeId);
        Task<BannerDto?> GetBannerByIdAsync(int id, int storeId);
        Task<BannerDto> CreateBannerAsync(CreateBannerRequest request, int storeId);
        Task<BannerDto?> UpdateBannerAsync(int id, UpdateBannerRequest request, int storeId);
        Task<bool> DeleteBannerAsync(int id, int storeId);
        Task<bool> ToggleBannerStatusAsync(int id, int storeId);
        
        // Brand Settings
        Task<BrandSettingsDto?> GetBrandSettingsAsync();
        Task<BrandSettingsDto> UpdateBrandSettingsAsync(UpdateBrandSettingsRequest request);
        
        // Store Information
        Task<StoreInfoDto> GetStoreInfoAsync();
        Task<StoreLocationDto> GetStoreLocationAsync();
        Task<StoreContactDto> GetStoreContactAsync();
        Task<StoreDescriptionDto> GetStoreDescriptionAsync();
        Task<List<BannerDto>> GetStoreBannersAsync();
        Task<List<CategoryDto>> GetStoreCategoriesAsync();
        Task<List<FilterDto>> GetStoreFiltersAsync();
        Task<List<FilterPresetDto>> GetStoreFilterPresetsAsync();
    }
} 