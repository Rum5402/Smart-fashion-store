using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Store;
using Fashion.Core.Enums;
using Fashion.Service.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    /// <summary>
    /// Controller for advanced product filtering functionality
    /// Provides comprehensive filtering capabilities with pagination, metadata, and filter presets
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductFilterController : ControllerBase
    {
        private readonly IProductFilterService _productFilterService;

        public ProductFilterController(IProductFilterService productFilterService)
        {
            _productFilterService = productFilterService;
        }

        /// <summary>
        /// Filter products based on multiple criteria with pagination
        /// </summary>
        /// <remarks>
        /// This endpoint provides advanced filtering capabilities for products with the following features:
        /// - Text search across product names, descriptions, and tags
        /// - Category filtering (Men, Women, Kids)
        /// - Style filtering (Casual, Formal, Sport, etc.)
        /// - Product type filtering (Clothing, Accessories, Shoes, etc.)
        /// - Size filtering (XS, S, M, L, XL, XXL)
        /// - Color filtering (Red, Blue, Black, etc.)
        /// - Brand name filtering
        /// - Fabric type filtering
        /// - Price range filtering
        /// - Promotion filtering (New Collection, Best Seller, On Sale)
        /// - Multiple sorting options
        /// - Pagination support
        /// 
        /// Example request:
        /// {
        ///   "searchTerm": "cotton",
        ///   "categories": ["Men", "Women"],
        ///   "styles": ["Casual"],
        ///   "sizes": ["M", "L"],
        ///   "colors": ["Blue", "Black"],
        ///   "minPrice": 50.0,
        ///   "maxPrice": 200.0,
        ///   "isNewCollection": true,
        ///   "sortOrder": "PriceLowToHigh",
        ///   "page": 1,
        ///   "pageSize": 20
        /// }
        /// </remarks>
        /// <param name="request">Filter criteria and pagination parameters</param>
        /// <returns>Filtered products with metadata and pagination info</returns>
        /// <response code="200">Products filtered successfully</response>
        /// <response code="400">Invalid request parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("filter")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> FilterProducts([FromBody] ProductFilterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid request parameters", errors = ModelState });

                var result = await _productFilterService.FilterProductsAsync(request);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Products filtered successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error filtering products", details = ex.Message });
            }
        }

        /// <summary>
        /// Get filter metadata for available options
        /// </summary>
        /// <remarks>
        /// This endpoint returns metadata about available filter options including:
        /// - Available categories with counts
        /// - Available styles with counts
        /// - Available product types with counts
        /// - Available sizes with counts
        /// - Available colors with counts
        /// - Available brand names with counts
        /// - Available fabric types with counts
        /// - Price range information
        /// 
        /// The metadata is calculated based on the current filter state, showing only options
        /// that would result in products being returned.
        /// </remarks>
        /// <param name="request">Current filter state to calculate available options</param>
        /// <returns>Available filter options with counts</returns>
        /// <response code="200">Filter metadata retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("metadata")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFilterMetadata([FromBody] ProductFilterRequest? request = null)
        {
            try
            {
                var metadata = await _productFilterService.GetFilterMetadataAsync(request);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Filter metadata retrieved successfully",
                    data = metadata
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving filter metadata", details = ex.Message });
            }
        }

        /// <summary>
        /// Get available filter options for a specific filter type
        /// </summary>
        /// <remarks>
        /// This endpoint returns available options for a specific filter type:
        /// - Size: Returns available sizes (XS, S, M, L, XL, XXL)
        /// - Color: Returns available colors (Red, Blue, Black, etc.)
        /// - Style: Returns available styles (Casual, Formal, Sport, etc.)
        /// - Type: Returns available product types (Clothing, Accessories, Shoes, etc.)
        /// - Promotion: Returns available promotions (New Collection, Best Seller, On Sale)
        /// 
        /// Each option includes the count of products that match that option.
        /// </remarks>
        /// <param name="filterType">Type of filter to get options for</param>
        /// <param name="currentFilters">Current filter state</param>
        /// <returns>Available options for the specified filter type</returns>
        /// <response code="200">Filter options retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("options/{filterType}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFilterOptions(
            [FromRoute] FilterType filterType, 
            [FromBody] ProductFilterRequest? currentFilters = null)
        {
            try
            {
                var options = await _productFilterService.GetFilterOptionsAsync(filterType, currentFilters);
                
                return Ok(new 
                { 
                    success = true, 
                    message = $"Filter options for {filterType} retrieved successfully",
                    data = options
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving filter options", details = ex.Message });
            }
        }

        /// <summary>
        /// Get price range information for all products
        /// </summary>
        /// <remarks>
        /// This endpoint returns price range information including:
        /// - Minimum price available
        /// - Maximum price available
        /// - Average price
        /// 
        /// The price range can be calculated based on current filter criteria.
        /// </remarks>
        /// <param name="request">Current filter state</param>
        /// <returns>Price range information</returns>
        /// <response code="200">Price range retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("price-range")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPriceRange([FromBody] ProductFilterRequest? request = null)
        {
            try
            {
                var priceRange = await _productFilterService.GetPriceRangeAsync(request);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Price range retrieved successfully",
                    data = priceRange
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving price range", details = ex.Message });
            }
        }

        /// <summary>
        /// Get quick filter options for common filter types
        /// </summary>
        /// <remarks>
        /// This endpoint returns quick filter options for the most commonly used filters:
        /// - Categories (Men, Women, Kids)
        /// - Sizes (XS, S, M, L, XL, XXL)
        /// - Colors (Red, Blue, Black, etc.)
        /// - Price range information
        /// 
        /// This is useful for building quick filter UI components.
        /// </remarks>
        /// <returns>Quick filter options</returns>
        /// <response code="200">Quick filters retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("quick-filters")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetQuickFilters()
        {
            try
            {
                var quickFilters = new
                {
                    Categories = await _productFilterService.GetFilterOptionsAsync(FilterType.Style),
                    Sizes = await _productFilterService.GetFilterOptionsAsync(FilterType.Size),
                    Colors = await _productFilterService.GetFilterOptionsAsync(FilterType.Color),
                    PriceRange = await _productFilterService.GetPriceRangeAsync()
                };
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Quick filters retrieved successfully",
                    data = quickFilters
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving quick filters", details = ex.Message });
            }
        }

        /// <summary>
        /// Get advanced filter options for detailed filtering
        /// </summary>
        /// <remarks>
        /// This endpoint returns advanced filter options for detailed filtering:
        /// - Product types (Clothing, Accessories, Shoes, etc.)
        /// - Store activities (Summer Collection, Winter Collection, etc.)
        /// - Promotions (New Collection, Best Seller, On Sale)
        /// - Price range information
        /// 
        /// This is useful for building advanced filter UI components.
        /// </remarks>
        /// <param name="request">Current filter state</param>
        /// <returns>Advanced filter options</returns>
        /// <response code="200">Advanced filters retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("advanced-filters")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAdvancedFilters([FromBody] ProductFilterRequest? request = null)
        {
            try
            {
                var advancedFilters = new
                {
                    ProductTypes = await _productFilterService.GetFilterOptionsAsync(FilterType.Type, request),
                    StoreActivities = await _productFilterService.GetFilterOptionsAsync(FilterType.Style, request),
                    Promotions = await _productFilterService.GetFilterOptionsAsync(FilterType.Promotion, request),
                    PriceRange = await _productFilterService.GetPriceRangeAsync(request)
                };
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Advanced filters retrieved successfully",
                    data = advancedFilters
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving advanced filters", details = ex.Message });
            }
        }
    }
} 