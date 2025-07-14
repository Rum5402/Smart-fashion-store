using System.Security.Claims;
using System.Text.Json;
using Fashion.Core.Enums;

namespace Fashion.Api.Middlewares
{
    public class WishlistAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public WishlistAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // التحقق من أن الطلب يذهب إلى Wishlist endpoints
            if (context.Request.Path.StartsWithSegments("/api/wishlist"))
            {
                // التحقق من وجود المستخدم
                if (!context.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        success = false,
                        message = "يجب تسجيل الدخول لاستخدام المفضلة",
                        error = "Unauthorized"
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }

                // التحقق من أن المستخدم ليس في Explore mode
                var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
                if (roleClaim == UserRole.Explore.ToString())
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        success = false,
                        message = "لا يمكن استخدام المفضلة في وضع التصفح. يرجى تسجيل الدخول كضيف أو إنشاء حساب.",
                        error = "Forbidden",
                        userRole = roleClaim
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }
            }

            await _next(context);
        }
    }

    // Extension method لتسهيل إضافة الـ middleware
    public static class WishlistAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseWishlistAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WishlistAuthorizationMiddleware>();
        }
    }
} 