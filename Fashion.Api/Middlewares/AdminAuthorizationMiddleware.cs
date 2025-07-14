using System.Security.Claims;
using System.Text.Json;

namespace Fashion.Api.Middlewares
{
    public class AdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // التحقق من أن الطلب يذهب إلى Admin Dashboard
            if (context.Request.Path.StartsWithSegments("/api/admin/dashboard"))
            {
                // التحقق من وجود المستخدم
                if (!context.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        success = false,
                        message = "غير مصرح لك بالوصول. يرجى تسجيل الدخول كمدير.",
                        error = "Unauthorized"
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }

                // التحقق من أن المستخدم مدير
                var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
                if (roleClaim != "Admin")
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        success = false,
                        message = "غير مصرح لك بالوصول. يجب أن تكون مدير للوصول لهذه الصفحة.",
                        error = "Forbidden",
                        userRole = roleClaim ?? "Unknown"
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }

                // إضافة معلومات المستخدم للـ headers للاستخدام لاحقاً
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var phoneNumber = context.User.FindFirst(ClaimTypes.Name)?.Value;
                
                context.Request.Headers["X-Admin-UserId"] = userId ?? "";
                context.Request.Headers["X-Admin-PhoneNumber"] = phoneNumber ?? "";
            }

            await _next(context);
        }
    }

    // Extension method لتسهيل إضافة الـ middleware
    public static class AdminAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminAuthorizationMiddleware>();
        }
    }
} 