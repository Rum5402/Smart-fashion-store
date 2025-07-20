using Fashion.Contract.DTOs.Store;
using Fashion.Core.Enums;

namespace Fashion.Service.Store
{
    /// <summary>
    /// Service interface for advanced product filtering functionality
    /// </summary>
    public interface IProductFilterService
    {
        /// <summary>
        /// Filter products based on multiple criteria with pagination
        /// </summary>
        /// <param name="request">Filter criteria and pagination parameters</param>
        /// <returns>Filtered products with metadata and pagination info</returns>
        Task<ProductFilterResponse> FilterProductsAsync(ProductFilterRequest request);
        
        /// <summary>
        /// Get filter metadata for available options
        /// </summary>
        /// <param name="request">Current filter state to calculate available options</param>
        /// <returns>Available filter options with counts</returns>
        Task<FilterMetadata> GetFilterMetadataAsync(ProductFilterRequest? request = null);
        
        /// <summary>
        /// Get available filter options for a specific filter type
        /// </summary>
        /// <param name="filterType">Type of filter to get options for</param>
        /// <param name="currentFilters">Current filter state</param>
        /// <returns>Available options for the specified filter type</returns>
        Task<List<FilterOption>> GetFilterOptionsAsync(FilterType filterType, ProductFilterRequest? currentFilters = null);
        
        /// <summary>
        /// Get price range information for all products
        /// </summary>
        /// <param name="request">Current filter state</param>
        /// <returns>Price range information</returns>
        Task<PriceRange> GetPriceRangeAsync(ProductFilterRequest? request = null);
    }
} 