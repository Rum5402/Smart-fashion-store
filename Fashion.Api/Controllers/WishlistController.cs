using Fashion.Contract.DTOs.Wishlist;
using Fashion.Service.Wishlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        /// <summary>
        /// الحصول على قائمة المفضلة للمستخدم
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<WishlistResponse>> GetWishlist()
        {
            try
            {
                var userId = GetCurrentUserId();
                var wishlist = await _wishlistService.GetUserWishlistAsync(userId);
                
                return Ok(new
                {
                    success = true,
                    message = "تم جلب قائمة المفضلة بنجاح",
                    data = wishlist
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء جلب قائمة المفضلة",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// إضافة منتج للمفضلة
        /// </summary>
        [HttpPost("add")]
        public async Task<ActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _wishlistService.AddToWishlistAsync(userId, request.ItemId);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "تم إضافة المنتج للمفضلة بنجاح"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "فشل في إضافة المنتج للمفضلة"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إضافة المنتج للمفضلة",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// إزالة منتج من المفضلة
        /// </summary>
        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveFromWishlist([FromBody] RemoveFromWishlistRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _wishlistService.RemoveFromWishlistAsync(userId, request.ItemId);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "تم إزالة المنتج من المفضلة بنجاح"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "فشل في إزالة المنتج من المفضلة"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إزالة المنتج من المفضلة",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من وجود منتج في المفضلة
        /// </summary>
        [HttpGet("check/{itemId}")]
        public async Task<ActionResult> IsInWishlist(int itemId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var isInWishlist = await _wishlistService.IsInWishlistAsync(userId, itemId);

                return Ok(new
                {
                    success = true,
                    data = new { isInWishlist }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء التحقق من المفضلة",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// طلب منتجات من المفضلة لغرفة القياس
        /// </summary>
        [HttpPost("request")]
        public async Task<ActionResult> RequestFromWishlist([FromBody] RequestFromWishlistRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _wishlistService.RequestFromWishlistAsync(userId, request);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "تم إرسال طلب غرفة القياس بنجاح"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "فشل في إرسال طلب غرفة القياس"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إرسال طلب غرفة القياس",
                    error = ex.Message
                });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }
    }
} 