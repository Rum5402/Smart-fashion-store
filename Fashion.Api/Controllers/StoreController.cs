using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Store;
using Fashion.Contract.DTOs.Common;
using Fashion.Service.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    /// <summary>
    /// Controller for store control center - managing categories, filters, banners, and brand settings
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class StoreController : BaseController
    {
        private readonly IStoreControlService _storeControlService;
        private readonly Fashion.Infrastructure.Data.FashionDbContext _context;

        public StoreController(IStoreControlService storeControlService, Fashion.Infrastructure.Data.FashionDbContext context)
        {
            _storeControlService = storeControlService;
            _context = context;
        }

        private async Task<int?> GetCurrentStoreIdAsync()
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            if (role == "Manager")
            {
                var manager = await _context.Managers.FindAsync(userId);
                return manager?.StoreId;
            }
            else if (role == "Customer" || role == "User")
            {
                var user = await _context.Users.FindAsync(userId);
                return user?.StoreId;
            }
            return null;
        }

        #region Categories Management

        /// <summary>
        /// Get all categories for management (hierarchical structure)
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var categories = await _storeControlService.GetAllCategoriesAsync(storeId.Value);
                return Ok(ApiResponse<List<CategoryDto>>.SuccessResponse(categories, "Categories retrieved successfully", categories.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CategoryDto>>.ErrorResponse("Error retrieving categories", ex.Message));
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var category = await _storeControlService.GetCategoryByIdAsync(id, storeId.Value);
                if (category == null)
                    return NotFound(ApiResponse<CategoryDto>.ErrorResponse("Category not found"));

                return Ok(ApiResponse<CategoryDto>.SuccessResponse(category, "Category retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("Error retrieving category", ex.Message));
            }
        }

        /// <summary>
        /// Create new category
        /// </summary>
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var category = await _storeControlService.CreateCategoryAsync(request, storeId.Value);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, 
                    ApiResponse<CategoryDto>.SuccessResponse(category, "Category created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("Error creating category", ex.Message));
            }
        }

        /// <summary>
        /// Update category
        /// </summary>
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var category = await _storeControlService.UpdateCategoryAsync(id, request, storeId.Value);
                if (category == null)
                    return NotFound(ApiResponse<CategoryDto>.ErrorResponse("Category not found"));

                return Ok(ApiResponse<CategoryDto>.SuccessResponse(category, "Category updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("Error updating category", ex.Message));
            }
        }

        /// <summary>
        /// Delete category
        /// </summary>
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.DeleteCategoryAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Category not found or cannot be deleted"));

                return Ok(ApiResponse.SuccessResponse("Category deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error deleting category", ex.Message));
            }
        }

        /// <summary>
        /// Toggle category status (active/inactive)
        /// </summary>
        [HttpPatch("categories/{id}/toggle-status")]
        public async Task<IActionResult> ToggleCategoryStatus(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.ToggleCategoryStatusAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Category not found"));

                return Ok(ApiResponse.SuccessResponse("Category status toggled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error toggling category status", ex.Message));
            }
        }

        #endregion

        #region Filters Management

        /// <summary>
        /// Get all filters for management
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetAllFilters()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var filters = await _storeControlService.GetAllFiltersAsync(storeId.Value);
                return Ok(ApiResponse<List<FilterDto>>.SuccessResponse(filters, "Filters retrieved successfully", filters.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<FilterDto>>.ErrorResponse("Error retrieving filters", ex.Message));
            }
        }

        /// <summary>
        /// Get filter by ID
        /// </summary>
        [HttpGet("filters/{id}")]
        public async Task<IActionResult> GetFilterById(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var filter = await _storeControlService.GetFilterByIdAsync(id, storeId.Value);
                if (filter == null)
                    return NotFound(ApiResponse<FilterDto>.ErrorResponse("Filter not found"));

                return Ok(ApiResponse<FilterDto>.SuccessResponse(filter, "Filter retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FilterDto>.ErrorResponse("Error retrieving filter", ex.Message));
            }
        }

        /// <summary>
        /// Create new filter
        /// </summary>
        [HttpPost("filters")]
        public async Task<IActionResult> CreateFilter([FromBody] CreateFilterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<FilterDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var filter = await _storeControlService.CreateFilterAsync(request, storeId.Value);
                return CreatedAtAction(nameof(GetFilterById), new { id = filter.Id }, 
                    ApiResponse<FilterDto>.SuccessResponse(filter, "Filter created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FilterDto>.ErrorResponse("Error creating filter", ex.Message));
            }
        }

        /// <summary>
        /// Update filter
        /// </summary>
        [HttpPut("filters/{id}")]
        public async Task<IActionResult> UpdateFilter(int id, [FromBody] UpdateFilterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<FilterDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var filter = await _storeControlService.UpdateFilterAsync(id, request, storeId.Value);
                if (filter == null)
                    return NotFound(ApiResponse<FilterDto>.ErrorResponse("Filter not found"));

                return Ok(ApiResponse<FilterDto>.SuccessResponse(filter, "Filter updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FilterDto>.ErrorResponse("Error updating filter", ex.Message));
            }
        }

        /// <summary>
        /// Delete filter
        /// </summary>
        [HttpDelete("filters/{id}")]
        public async Task<IActionResult> DeleteFilter(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.DeleteFilterAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Filter not found or cannot be deleted"));

                return Ok(ApiResponse.SuccessResponse("Filter deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error deleting filter", ex.Message));
            }
        }

        /// <summary>
        /// Toggle filter status (active/inactive)
        /// </summary>
        [HttpPatch("filters/{id}/toggle-status")]
        public async Task<IActionResult> ToggleFilterStatus(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.ToggleFilterStatusAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Filter not found"));

                return Ok(ApiResponse.SuccessResponse("Filter status toggled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error toggling filter status", ex.Message));
            }
        }

        #endregion

        #region Banners Management

        /// <summary>
        /// Get all banners for management
        /// </summary>
        [HttpGet("banners")]
        public async Task<IActionResult> GetAllBanners()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var banners = await _storeControlService.GetAllBannersAsync(storeId.Value);
                return Ok(ApiResponse<List<BannerDto>>.SuccessResponse(banners, "Banners retrieved successfully", banners.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<BannerDto>>.ErrorResponse("Error retrieving banners", ex.Message));
            }
        }

        /// <summary>
        /// Get banner by ID
        /// </summary>
        [HttpGet("banners/{id}")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var banner = await _storeControlService.GetBannerByIdAsync(id, storeId.Value);
                if (banner == null)
                    return NotFound(ApiResponse<BannerDto>.ErrorResponse("Banner not found"));

                return Ok(ApiResponse<BannerDto>.SuccessResponse(banner, "Banner retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BannerDto>.ErrorResponse("Error retrieving banner", ex.Message));
            }
        }

        /// <summary>
        /// Create new banner
        /// </summary>
        [HttpPost("banners")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<BannerDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var banner = await _storeControlService.CreateBannerAsync(request, storeId.Value);
                return CreatedAtAction(nameof(GetBannerById), new { id = banner.Id }, 
                    ApiResponse<BannerDto>.SuccessResponse(banner, "Banner created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BannerDto>.ErrorResponse("Error creating banner", ex.Message));
            }
        }

        /// <summary>
        /// Update banner
        /// </summary>
        [HttpPut("banners/{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<BannerDto>.ErrorResponse("Invalid request data"));
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var banner = await _storeControlService.UpdateBannerAsync(id, request, storeId.Value);
                if (banner == null)
                    return NotFound(ApiResponse<BannerDto>.ErrorResponse("Banner not found"));

                return Ok(ApiResponse<BannerDto>.SuccessResponse(banner, "Banner updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BannerDto>.ErrorResponse("Error updating banner", ex.Message));
            }
        }

        /// <summary>
        /// Delete banner
        /// </summary>
        [HttpDelete("banners/{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.DeleteBannerAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Banner not found or cannot be deleted"));

                return Ok(ApiResponse.SuccessResponse("Banner deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error deleting banner", ex.Message));
            }
        }

        /// <summary>
        /// Toggle banner status (active/inactive)
        /// </summary>
        [HttpPatch("banners/{id}/toggle-status")]
        public async Task<IActionResult> ToggleBannerStatus(int id)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var result = await _storeControlService.ToggleBannerStatusAsync(id, storeId.Value);
                if (!result)
                    return NotFound(ApiResponse.ErrorResponse("Banner not found"));

                return Ok(ApiResponse.SuccessResponse("Banner status toggled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResponse("Error toggling banner status", ex.Message));
            }
        }

        #endregion

        #region Brand Settings Management

        /// <summary>
        /// Get brand settings
        /// </summary>
        [HttpGet("brand-settings")]
        public async Task<IActionResult> GetBrandSettings()
        {
            try
            {
                var brandSettings = await _storeControlService.GetBrandSettingsAsync();
                if (brandSettings == null)
                    return NotFound(ApiResponse<BrandSettingsDto>.ErrorResponse("Brand settings not found"));

                return Ok(ApiResponse<BrandSettingsDto>.SuccessResponse(brandSettings, "Brand settings retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BrandSettingsDto>.ErrorResponse("Error retrieving brand settings", ex.Message));
            }
        }

        /// <summary>
        /// Update brand settings
        /// </summary>
        [HttpPut("brand-settings")]
        public async Task<IActionResult> UpdateBrandSettings([FromBody] UpdateBrandSettingsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<BrandSettingsDto>.ErrorResponse("Invalid request data"));

                var brandSettings = await _storeControlService.UpdateBrandSettingsAsync(request);
                return Ok(ApiResponse<BrandSettingsDto>.SuccessResponse(brandSettings, "Brand settings updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BrandSettingsDto>.ErrorResponse("Error updating brand settings", ex.Message));
            }
        }

        #endregion

        #region Public Endpoints (Anonymous Access)

        /// <summary>
        /// Get store information including brand name, location, and contact details
        /// </summary>
        [HttpGet("info")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreInfo()
        {
            try
            {
                var storeInfo = await _storeControlService.GetStoreInfoAsync();
                return Ok(ApiResponse<StoreInfoDto>.SuccessResponse(storeInfo, "Store information retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<StoreInfoDto>.ErrorResponse("Error retrieving store information", ex.Message));
            }
        }

        /// <summary>
        /// Get store location and contact information
        /// </summary>
        [HttpGet("location")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreLocation()
        {
            try
            {
                var location = await _storeControlService.GetStoreLocationAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Store location retrieved successfully",
                    data = location
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving store location", details = ex.Message });
            }
        }

        /// <summary>
        /// Get store contact information
        /// </summary>
        [HttpGet("contact")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreContact()
        {
            try
            {
                var contact = await _storeControlService.GetStoreContactAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Store contact information retrieved successfully",
                    data = contact
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving store contact", details = ex.Message });
            }
        }

        /// <summary>
        /// Get store description and about information
        /// </summary>
        [HttpGet("description")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreDescription()
        {
            try
            {
                var description = await _storeControlService.GetStoreDescriptionAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Store description retrieved successfully",
                    data = description
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving store description", details = ex.Message });
            }
        }

        /// <summary>
        /// Get store banners and promotional content (public)
        /// </summary>
        [HttpGet("banners/public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreBanners()
        {
            try
            {
                var banners = await _storeControlService.GetStoreBannersAsync();
                return Ok(ApiResponse<IEnumerable<BannerDto>>.SuccessResponse(banners, "Store banners retrieved successfully", banners.Count()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<BannerDto>>.ErrorResponse("Error retrieving store banners", ex.Message));
            }
        }

        /// <summary>
        /// Get store categories (public)
        /// </summary>
        [HttpGet("categories/public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreCategories()
        {
            try
            {
                var categories = await _storeControlService.GetStoreCategoriesAsync();
                return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories, "Store categories retrieved successfully", categories.Count()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<CategoryDto>>.ErrorResponse("Error retrieving store categories", ex.Message));
            }
        }

        /// <summary>
        /// Get store filters (public)
        /// </summary>
        [HttpGet("filters/public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreFilters()
        {
            try
            {
                var filters = await _storeControlService.GetStoreFiltersAsync();
                return Ok(ApiResponse<IEnumerable<FilterDto>>.SuccessResponse(filters, "Store filters retrieved successfully", filters.Count()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<FilterDto>>.ErrorResponse("Error retrieving store filters", ex.Message));
            }
        }

        /// <summary>
        /// Get store filter presets
        /// </summary>
        [HttpGet("filter-presets")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreFilterPresets()
        {
            try
            {
                var filterPresets = await _storeControlService.GetStoreFilterPresetsAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Store filter presets retrieved successfully",
                    data = filterPresets
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving store filter presets", details = ex.Message });
            }
        }

        #endregion
    }
} 