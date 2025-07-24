using Fashion.Contract.DTOs.Store;
using Fashion.Contract.DTOs.Items;

namespace Fashion.Service.Store
{
    /// <summary>
    /// Interface for Home page service functionality
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Get complete home page data including banners, featured products, and categories
        /// </summary>
        Task<HomePageDto> GetHomePageDataAsync(int storeId);

        /// <summary>
        /// Get shop by category data with filter options
        /// </summary>
        Task<ShopByCategoryDto> GetShopByCategoryAsync(string category, int storeId);

        /// <summary>
        /// Get filter options for a specific category
        /// </summary>
        Task<CategoryFilterOptionsDto> GetFilterOptionsForCategoryAsync(string category, int storeId);

        /// <summary>
        /// Get banners for home page
        /// </summary>
        Task<List<BannerDto>> GetBannersAsync();

        /// <summary>
        /// Get store categories
        /// </summary>
        Task<List<Fashion.Contract.DTOs.Store.CategoryDto>> GetCategoriesAsync();
    }
} 