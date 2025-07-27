using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Fashion.Api.Extensions;

namespace Fashion.Api.Middlewares
{
    public class StoreDomainMiddleware
    {
        private readonly RequestDelegate _next;

        public StoreDomainMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, FashionDbContext dbContext)
        {
            var host = context.Request.Host.Value; // مثل: "cairo-mall.fashion.com"
            
            var store = await dbContext.StoreBrandSettings
                .FirstOrDefaultAsync(s => s.StoreDomain == host && s.IsActive);
                
            if (store != null)
            {
                context.Items["CurrentStoreId"] = store.Id;
                context.Items["CurrentStore"] = store;
            }
            
            await _next(context);
        }
    }
} 