using Fashion.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Fashion.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static int? GetCurrentStoreId(this HttpContext context)
        {
            return context.Items["CurrentStoreId"] as int?;
        }
        
        public static StoreBrandSettings? GetCurrentStore(this HttpContext context)
        {
            return context.Items["CurrentStore"] as StoreBrandSettings;
        }
    }
} 