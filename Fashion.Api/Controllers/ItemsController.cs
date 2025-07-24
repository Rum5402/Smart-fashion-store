using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Items;
using Fashion.Core.Enums;
using Fashion.Service.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    /// <summary>
    /// Controller for product management and retrieval
    /// Provides endpoints for getting products with various filters and categories
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ItemsController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly Fashion.Infrastructure.Data.FashionDbContext _context;
        public ItemsController(IItemService itemService, Fashion.Infrastructure.Data.FashionDbContext context)
        {
            _itemService = itemService;
            _context = context;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                int storeId;
                var role = GetCurrentUserRole();
                var userId = GetCurrentUserId();
                if (role == "Manager")
                {
                    var manager = await _context.Managers.FindAsync(userId);
                    if (manager == null)
                        return ErrorResponse("Manager not found");
                    storeId = manager.StoreId;
                }
                else if (role == "Customer" || role == "User")
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                        return ErrorResponse("User not found");
                    storeId = user.StoreId;
                }
                else
                {
                    return ErrorResponse("User role not supported for product listing");
                }
                var items = await _itemService.GetAllItemsAsync(storeId);
                return SuccessResponse(items, "Products retrieved successfully", items.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving products", ex.Message);
            }
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                    return NotFoundResponse("Product not found");

                return SuccessResponse(item, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving product", ex.Message);
            }
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

        /// <summary>
        /// Get new collection products
        /// </summary>
        [HttpGet("new-collection")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetNewCollection()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetNewCollectionAsync(storeId.Value);
                return Ok(new { success = true, message = "New collection products retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving new collection", details = ex.Message });
            }
        }

        /// <summary>
        /// Get best sellers products
        /// </summary>
        [HttpGet("best-sellers")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBestSellers()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetBestSellersAsync(storeId.Value);
                return Ok(new { success = true, message = "Best sellers products retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving best sellers", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products on sale
        /// </summary>
        [HttpGet("on-sale")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetOnSale()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetOnSaleAsync(storeId.Value);
                return Ok(new { success = true, message = "On sale products retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving on sale products", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products by category (Men, Women, Kids)
        /// </summary>
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItemsByCategory(string category)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetItemsByCategoryAsync(category, storeId.Value);
                return Ok(new { success = true, message = "Products by category retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving products by category", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products by product type (T-Shirt, Sweatpants, Pants, Shirt, Shoes, etc.)
        /// </summary>
        [HttpGet("type/{productType}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItemsByProductType(string productType)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetItemsByProductTypeAsync(productType, storeId.Value);
                return Ok(new { success = true, message = "Products by product type retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving products by product type", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products by style (Casual, Formal, Sport, Outing)
        /// </summary>
        [HttpGet("style/{style}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItemsByStyle(string style)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetItemsByStyleAsync(style, storeId.Value);
                return Ok(new { success = true, message = "Products by style retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving products by style", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products by color
        /// </summary>
        [HttpGet("color/{color}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItemsByColor(string color)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetItemsByColorAsync(color, storeId.Value);
                return Ok(new { success = true, message = "Products by color retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving products by color", details = ex.Message });
            }
        }

        /// <summary>
        /// Get products by price range
        /// </summary>
        [HttpGet("price-range")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItemsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetItemsByPriceRangeAsync(minPrice, maxPrice, storeId.Value);
                return Ok(new { success = true, message = "Products by price range retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving products by price range", details = ex.Message });
            }
        }

        /// <summary>
        /// Get product counts by category
        /// </summary>
        [HttpGet("counts/categories")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProductCountsByCategory()
        {
            try
            {
                var counts = await _itemService.GetProductCountsByCategoryAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Product counts by category retrieved successfully",
                    data = counts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving product counts by category", details = ex.Message });
            }
        }

        /// <summary>
        /// Get product counts by product type
        /// </summary>
        [HttpGet("counts/product-types")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProductCountsByProductType()
        {
            try
            {
                var counts = await _itemService.GetProductCountsByProductTypeAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Product counts by product type retrieved successfully",
                    data = counts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving product counts by product type", details = ex.Message });
            }
        }

        /// <summary>
        /// Get product counts by style
        /// </summary>
        [HttpGet("counts/styles")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProductCountsByStyle()
        {
            try
            {
                var counts = await _itemService.GetProductCountsByStyleAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Product counts by style retrieved successfully",
                    data = counts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving product counts by style", details = ex.Message });
            }
        }

        /// <summary>
        /// Get product counts by color
        /// </summary>
        [HttpGet("counts/colors")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProductCountsByColor()
        {
            try
            {
                var counts = await _itemService.GetProductCountsByColorAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Product counts by color retrieved successfully",
                    data = counts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving product counts by color", details = ex.Message });
            }
        }

        /// <summary>
        /// Get price range statistics
        /// </summary>
        [HttpGet("price-statistics")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetPriceStatistics()
        {
            try
            {
                var statistics = await _itemService.GetPriceStatisticsAsync();
                return Ok(new 
                { 
                    success = true, 
                    message = "Price statistics retrieved successfully",
                    data = statistics
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving price statistics", details = ex.Message });
            }
        }

        /// <summary>
        /// Search products by name or description
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.SearchProductsAsync(query, storeId.Value);
                return Ok(new { success = true, message = "Search results retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error searching products", details = ex.Message });
            }
        }

        /// <summary>
        /// Get featured products (combination of new collection, best sellers, and on sale)
        /// </summary>
        [HttpGet("featured")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            try
            {
                var storeId = await GetCurrentStoreIdAsync();
                if (storeId == null)
                    return ErrorResponse("StoreId not found for current user");
                var items = await _itemService.GetFeaturedProductsAsync(storeId.Value);
                return Ok(new { success = true, message = "Featured products retrieved successfully", data = items, totalCount = items.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving featured products", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequestResponse("Invalid request data");

                var createdItem = await _itemService.CreateItemAsync(request);
                return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, new
                {
                    success = true,
                    message = "Product created successfully",
                    data = createdItem
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error creating product", ex.Message);
            }
        }

        /// <summary>
        /// Get available categories for product creation
        /// </summary>
        [HttpGet("form-options/categories")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = Enum.GetValues(typeof(ItemCategory))
                    .Cast<ItemCategory>()
                    .Select(c => new { id = (int)c, name = c.ToString() })
                    .ToList();

                return SuccessResponse(categories, "Categories retrieved successfully", categories.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving categories", ex.Message);
            }
        }

        /// <summary>
        /// Get available product types for product creation
        /// </summary>
        [HttpGet("form-options/product-types")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetProductTypes()
        {
            try
            {
                var productTypes = Enum.GetValues(typeof(ProductType))
                    .Cast<ProductType>()
                    .Select(p => new { id = (int)p, name = p.ToString() })
                    .ToList();

                return SuccessResponse(productTypes, "Product types retrieved successfully", productTypes.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving product types", ex.Message);
            }
        }

        /// <summary>
        /// Get available styles for product creation
        /// </summary>
        [HttpGet("form-options/styles")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetStyles()
        {
            try
            {
                var styles = Enum.GetValues(typeof(ItemStyle))
                    .Cast<ItemStyle>()
                    .Select(s => new { id = (int)s, name = s.ToString() })
                    .ToList();

                return SuccessResponse(styles, "Styles retrieved successfully", styles.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving styles", ex.Message);
            }
        }

        /// <summary>
        /// Get available sizes for product creation
        /// </summary>
        [HttpGet("form-options/sizes")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetAvailableSizes()
        {
            try
            {
                var sizes = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };
                return SuccessResponse(sizes, "Available sizes retrieved successfully", sizes.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving sizes", ex.Message);
            }
        }

        /// <summary>
        /// Get available colors for product creation
        /// </summary>
        [HttpGet("form-options/colors")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetAvailableColors()
        {
            try
            {
                var colors = new List<string> 
                { 
                    "Red", "Orange", "Brown", "Purple", "Pink", 
                    "Light Blue", "Dark Blue", "Green", "Light Yellow", 
                    "White", "Black", "Gray", "Navy", "Beige", "Olive" 
                };
                return SuccessResponse(colors, "Available colors retrieved successfully", colors.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving colors", ex.Message);
            }
        }

        /// <summary>
        /// Get suggested tags for product creation
        /// </summary>
        [HttpGet("form-options/suggested-tags")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetSuggestedTags()
        {
            try
            {
                var suggestedTags = new List<string>
                {
                    "Formal", "Casual", "Sport", "Outing", "Classic", "Sporty",
                    "Smart Casual", "Chic", "Athleisure", "Street Style", "Minimalist",
                    "Bohemian", "Edgy", "Vintage", "Modern", "Elegant", "Comfortable"
                };
                return SuccessResponse(suggestedTags, "Suggested tags retrieved successfully", suggestedTags.Count);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving suggested tags", ex.Message);
            }
        }

        /// <summary>
        /// Get all form options for product creation in one request
        /// </summary>
        [HttpGet("form-options")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetFormOptions()
        {
            try
            {
                var categories = Enum.GetValues(typeof(ItemCategory))
                    .Cast<ItemCategory>()
                    .Select(c => new { id = (int)c, name = c.ToString() })
                    .ToList();

                var productTypes = Enum.GetValues(typeof(ProductType))
                    .Cast<ProductType>()
                    .Select(p => new { id = (int)p, name = p.ToString() })
                    .ToList();

                var styles = Enum.GetValues(typeof(ItemStyle))
                    .Cast<ItemStyle>()
                    .Select(s => new { id = (int)s, name = s.ToString() })
                    .ToList();

                var sizes = new List<string> { "XS", "S", "M", "L", "XL", "XXL" };
                
                var colors = new List<string> 
                { 
                    "Red", "Orange", "Brown", "Purple", "Pink", 
                    "Light Blue", "Dark Blue", "Green", "Light Yellow", 
                    "White", "Black", "Gray", "Navy", "Beige", "Olive" 
                };

                var suggestedTags = new List<string>
                {
                    "Formal", "Casual", "Sport", "Outing", "Classic", "Sporty",
                    "Smart Casual", "Chic", "Athleisure", "Street Style", "Minimalist",
                    "Bohemian", "Edgy", "Vintage", "Modern", "Elegant", "Comfortable"
                };

                var formOptions = new
                {
                    categories,
                    productTypes,
                    styles,
                    sizes,
                    colors,
                    suggestedTags
                };

                return SuccessResponse(formOptions, "Form options retrieved successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error retrieving form options", ex.Message);
            }
        }
    }
} 