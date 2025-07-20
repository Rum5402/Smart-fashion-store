using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Items;
using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Admin;
using Fashion.Contract.DTOs.Store;
using Fashion.Service.Items;
using Fashion.Service.Admin;
using Fashion.Service.Store;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Fashion.Service.JWT;
using Fashion.Service.FittingRoomServices;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/manager/dashboard")]
    [AuthorizeRoles("Manager")]
    public class ManagerDashboardController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IJwtService _jwtService;
        private readonly IStoreManagementService _storeManagementService;
        private readonly IStoreControlService _storeControlService;
        private readonly IFittingRoomService _fittingRoomService;

        public ManagerDashboardController(IItemService itemService, IJwtService jwtService, IStoreManagementService storeManagementService, IStoreControlService storeControlService, IFittingRoomService fittingRoomService)
        {
            _itemService = itemService;
            _jwtService = jwtService;
            _storeManagementService = storeManagementService;
            _storeControlService = storeControlService;
            _fittingRoomService = fittingRoomService;
        }

        #region Dashboard Overview
        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            try
            {
                var allItems = await _itemService.GetAllItemsAsync();

                var overview = new
                {
                    TotalProducts = allItems.Count,
                    NewCollectionCount = allItems.Count(i => i.IsNewCollection),
                    BestSellersCount = allItems.Count(i => i.IsBestSeller),
                    OnSaleCount = allItems.Count(i => i.IsOnSale),
                    ActiveProducts = allItems.Count(i => i.IsActive),
                    ProductsByCategory = allItems.GroupBy(i => i.Category).Select(g => new { Category = g.Key, Count = g.Count() }),
                    ProductsByStyle = allItems.GroupBy(i => i.Style).Select(g => new { Style = g.Key, Count = g.Count() }),
                    ProductsByType = allItems.GroupBy(i => i.ProductType).Select(g => new { ProductType = g.Key, Count = g.Count() }),
                    ProductsByStoreActivity = allItems.GroupBy(i => i.StoreActivity).Select(g => new { StoreActivity = g.Key, Count = g.Count() })
                };

                return Ok(overview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error retrieving dashboard overview", details = ex.Message });
            }
        }
        #endregion

        #region Analytics and Reports
        [HttpGet("analytics/inventory")]
        public async Task<IActionResult> GetInventoryAnalytics()
        {
            try
            {
                var allItems = await _itemService.GetAllItemsAsync();

                var analytics = new
                {
                    TotalProducts = allItems.Count,
                    ActiveProducts = allItems.Count(i => i.IsActive),
                    InactiveProducts = allItems.Count(i => !i.IsActive),
                    NewCollectionProducts = allItems.Count(i => i.IsNewCollection),
                    BestSellers = allItems.Count(i => i.IsBestSeller),
                    OnSaleProducts = allItems.Count(i => i.IsOnSale),
                    ProductsByCategory = allItems.GroupBy(i => i.Category)
                        .Select(g => new { Category = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count),
                    ProductsByStyle = allItems.GroupBy(i => i.Style)
                        .Select(g => new { Style = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count),
                    AveragePrice = allItems.Any() ? allItems.Average(i => i.Price) : 0,
                    PriceRange = new
                    {
                        Min = allItems.Any() ? allItems.Min(i => i.Price) : 0,
                        Max = allItems.Any() ? allItems.Max(i => i.Price) : 0
                    }
                };

                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error retrieving inventory analytics", details = ex.Message });
            }
        }
        #endregion

        #region Product Management
        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.CreateItemAsync(request);
                return Created($"/api/items/{item.Id}", new { success = true, message = "Product created successfully", product = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error creating product", details = ex.Message });
            }
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.UpdateItemAsync(id, request);
                if (item == null)
                    return NotFound(new { success = false, message = "Product not found" });

                return Ok(new { success = true, message = "Product updated successfully", product = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating product", details = ex.Message });
            }
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _itemService.DeleteItemAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Product not found" });

                return Ok(new { success = true, message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error deleting product", details = ex.Message });
            }
        }

        // Note: Product retrieval endpoints are handled by ItemsController
        // Use /api/items for getting products

        [HttpPut("products/{id}/toggle-status")]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {
            try
            {
                var success = await _itemService.ToggleItemStatusAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Product not found" });

                return Ok(new { success = true, message = "Product status toggled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error toggling product status", details = ex.Message });
            }
        }


        #endregion

        #region Store Management
        [HttpGet("store/settings")]
        public async Task<IActionResult> GetStoreSettings()
        {
            try
            {
                var result = await _storeManagementService.GetStoreSettingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving store settings", details = ex.Message });
            }
        }

        [HttpPut("store/settings")]
        public async Task<IActionResult> UpdateStoreSettings([FromBody] StoreActivityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _storeManagementService.UpdateStoreSettingsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating store settings", details = ex.Message });
            }
        }



        [HttpGet("store/product-types")]
        public async Task<IActionResult> GetAvailableProductTypes()
        {
            try
            {
                var result = await _storeManagementService.GetAvailableProductTypesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving available product types", details = ex.Message });
            }
        }

        [HttpGet("store/product-styles")]
        public async Task<IActionResult> GetAvailableProductStyles()
        {
            try
            {
                var result = await _storeManagementService.GetAvailableProductStylesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving available product styles", details = ex.Message });
            }
        }
        #endregion

        #region Store Control Center - Categories
        // Note: Category retrieval endpoints are handled by StoreController
        // Use /api/store/categories for getting categories

        [HttpPost("store/categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var category = await _storeControlService.CreateCategoryAsync(request);
                return Created($"/api/store/categories/{category.Id}", new { success = true, message = "Category created successfully", category = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error creating category", details = ex.Message });
            }
        }

        [HttpPut("store/categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var category = await _storeControlService.UpdateCategoryAsync(id, request);
                if (category == null)
                    return NotFound(new { success = false, message = "Category not found" });

                return Ok(new { success = true, message = "Category updated successfully", category = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating category", details = ex.Message });
            }
        }

        [HttpDelete("store/categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _storeControlService.DeleteCategoryAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Category not found or has subcategories" });

                return Ok(new { success = true, message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error deleting category", details = ex.Message });
            }
        }

        [HttpPut("store/categories/{id}/toggle-status")]
        public async Task<IActionResult> ToggleCategoryStatus(int id)
        {
            try
            {
                var success = await _storeControlService.ToggleCategoryStatusAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Category not found" });

                return Ok(new { success = true, message = "Category status toggled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error toggling category status", details = ex.Message });
            }
        }
        #endregion

        #region Store Control Center - Filters
        // Note: Filter retrieval endpoints are handled by StoreController
        // Use /api/store/filters for getting filters

        [HttpPost("store/filters")]
        public async Task<IActionResult> CreateFilter([FromBody] CreateFilterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var filter = await _storeControlService.CreateFilterAsync(request);
                return Created($"/api/store/filters/{filter.Id}", new { success = true, message = "Filter created successfully", filter = filter });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error creating filter", details = ex.Message });
            }
        }

        [HttpPut("store/filters/{id}")]
        public async Task<IActionResult> UpdateFilter(int id, [FromBody] UpdateFilterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var filter = await _storeControlService.UpdateFilterAsync(id, request);
                if (filter == null)
                    return NotFound(new { success = false, message = "Filter not found" });

                return Ok(new { success = true, message = "Filter updated successfully", filter = filter });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating filter", details = ex.Message });
            }
        }

        [HttpDelete("store/filters/{id}")]
        public async Task<IActionResult> DeleteFilter(int id)
        {
            try
            {
                var success = await _storeControlService.DeleteFilterAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Filter not found" });

                return Ok(new { success = true, message = "Filter deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error deleting filter", details = ex.Message });
            }
        }

        [HttpPut("store/filters/{id}/toggle-status")]
        public async Task<IActionResult> ToggleFilterStatus(int id)
        {
            try
            {
                var success = await _storeControlService.ToggleFilterStatusAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Filter not found" });

                return Ok(new { success = true, message = "Filter status toggled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error toggling filter status", details = ex.Message });
            }
        }
        #endregion

        #region Store Control Center - Banners
        // Note: Banner retrieval endpoints are handled by StoreController
        // Use /api/store/banners for getting banners

        [HttpPost("store/banners")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var banner = await _storeControlService.CreateBannerAsync(request);
                return Created($"/api/store/banners/{banner.Id}", new { success = true, message = "Banner created successfully", banner = banner });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error creating banner", details = ex.Message });
            }
        }

        [HttpPut("store/banners/{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var banner = await _storeControlService.UpdateBannerAsync(id, request);
                if (banner == null)
                    return NotFound(new { success = false, message = "Banner not found" });

                return Ok(new { success = true, message = "Banner updated successfully", banner = banner });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating banner", details = ex.Message });
            }
        }

        [HttpDelete("store/banners/{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                var success = await _storeControlService.DeleteBannerAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Banner not found" });

                return Ok(new { success = true, message = "Banner deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error deleting banner", details = ex.Message });
            }
        }

        [HttpPut("store/banners/{id}/toggle-status")]
        public async Task<IActionResult> ToggleBannerStatus(int id)
        {
            try
            {
                var success = await _storeControlService.ToggleBannerStatusAsync(id);
                if (!success)
                    return NotFound(new { success = false, message = "Banner not found" });

                return Ok(new { success = true, message = "Banner status toggled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error toggling banner status", details = ex.Message });
            }
        }
        #endregion

        #region Store Control Center - Brand Settings
        [HttpGet("store/brand-settings")]
        public async Task<IActionResult> GetBrandSettings()
        {
            try
            {
                var settings = await _storeControlService.GetBrandSettingsAsync();
                if (settings == null)
                    return NotFound(new { success = false, message = "Brand settings not found" });

                return Ok(new { success = true, settings = settings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving brand settings", details = ex.Message });
            }
        }

        [HttpPut("store/brand-settings")]
        public async Task<IActionResult> UpdateBrandSettings([FromBody] UpdateBrandSettingsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var settings = await _storeControlService.UpdateBrandSettingsAsync(request);
                return Ok(new { success = true, message = "Brand settings updated successfully", settings = settings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating brand settings", details = ex.Message });
            }
        }
        #endregion

        #region Requests
        [HttpGet("requests")]
        public async Task<IActionResult> GetFittingRoomRequests()
        {
            try
            {
                var requests = await _fittingRoomService.GetAllRequestsAsync();
                var requestDtos = requests.Select(r => new Fashion.Contract.DTOs.Admin.FittingRoomRequestDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.User?.Name ?? "",
                    UserPhoneNumber = r.User?.PhoneNumber ?? "",
                    ItemId = r.ItemId,
                    ItemName = r.Item?.Name ?? "",
                    ItemImageUrl = r.Item?.ImageUrls ?? "",
                    ItemPrice = r.Item?.Price ?? 0,
                    Status = r.Status,
                    StatusDisplayName = r.Status.GetDisplayName(),
                    StaffMessage = r.StaffMessage,
                    HandledByStaffId = r.HandledByStaffId,
                    HandledByStaffName = "", // Will be populated from staff table if needed
                    HandledAt = r.HandledAt,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList();

                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving fitting room requests", details = ex.Message });
            }
        }
        #endregion
    }
} 