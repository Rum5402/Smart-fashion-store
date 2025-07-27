using Fashion.Contract.Interface;
using Fashion.Api.Extensions;
using Microsoft.AspNetCore.Http;

namespace Fashion.Api.Services
{
    public class StoreContextService : IStoreContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentStoreId()
        {
            return _httpContextAccessor.HttpContext?.GetCurrentStoreId();
        }

        public string? GetCurrentStoreDomain()
        {
            return _httpContextAccessor.HttpContext?.Request.Host.Value;
        }
    }
} 