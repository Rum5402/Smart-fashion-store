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
    public class ManagerDashboardController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly IJwtService _jwtService;
        private readonly IStoreManagementService _storeManagementService;
        private readonly IStoreControlService _storeControlService;
        private readonly IFittingRoomService _fittingRoomService;
        private readonly Fashion.Infrastructure.Data.FashionDbContext _context;

        public ManagerDashboardController(IItemService itemService, IJwtService jwtService, IStoreManagementService storeManagementService, IStoreControlService storeControlService, IFittingRoomService fittingRoomService, Fashion.Infrastructure.Data.FashionDbContext context)
        {
            _itemService = itemService;
            _jwtService = jwtService;
            _storeManagementService = storeManagementService;
            _storeControlService = storeControlService;
            _fittingRoomService = fittingRoomService;
            _context = context;
        }

        #region Dashboard Overview
        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            try
            {
                var userId = GetCurrentUserId();
                var manager = await _context.Managers.FindAsync(userId);
                if (manager == null)
                    return StatusCode(404, new { error = "Manager not found" });
                int storeId = manager.StoreId;
                var allItems = await _itemService.GetAllItemsAsync(storeId);

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
                var userId = GetCurrentUserId();
                var manager = await _context.Managers.FindAsync(userId);
                if (manager == null)
                    return StatusCode(404, new { error = "Manager not found" });
                int storeId = manager.StoreId;
                var allItems = await _itemService.GetAllItemsAsync(storeId);

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

            var userId = GetCurrentUserId();
            var manager = await _context.Managers.FindAsync(userId);
            if (manager == null)
                return Unauthorized(new { success = false, message = "Manager not found" });

            var item = await _context.Items.FindAsync(id);
            if (item == null || item.IsDeleted)
                return NotFound(new { success = false, message = "Product not found" });

            if (item.StoreId != manager.StoreId)
                return Unauthorized(new { success = false, message = "You do not have permission to modify this product" });

            try
            {
                var updated = await _itemService.UpdateItemAsync(id, request);
                return Ok(new { success = true, message = "Product updated successfully", product = updated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error updating product", details = ex.Message });
            }
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = GetCurrentUserId();
            var manager = await _context.Managers.FindAsync(userId);
            if (manager == null)
                return Unauthorized(new { success = false, message = "Manager not found" });

            var item = await _context.Items.FindAsync(id);
            if (item == null || item.IsDeleted)
                return NotFound(new { success = false, message = "Product not found" });

            if (item.StoreId != manager.StoreId)
                return Unauthorized(new { success = false, message = "You do not have permission to delete this product" });

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