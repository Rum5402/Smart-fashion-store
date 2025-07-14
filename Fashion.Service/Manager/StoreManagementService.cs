using Fashion.Contract.DTOs.Admin;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.Admin
{
    public class StoreManagementService : IStoreManagementService
    {
        private readonly FashionDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StoreManagementService(FashionDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<StoreActivityResponse> GetStoreSettingsAsync()
        {
            try
            {
                var settings = await _context.StoreSettings
                    .Where(s => s.IsActive && !s.IsDeleted)
                    .OrderByDescending(s => s.CreatedAt)
                    .FirstOrDefaultAsync();

                if (settings == null)
                {
                    // Create default settings if none exist
                    settings = new StoreSettings
                    {
                        StoreActivities = "[]",
                        ProductType = "All",
                        ProductStyle = "All",
                        IsActive = true
                    };
                    _context.StoreSettings.Add(settings);
                    await _context.SaveChangesAsync();
                }

                return new StoreActivityResponse
                {
                    Success = true,
                    Message = "Store settings retrieved successfully",
                    Data = new StoreActivityInfo
                    {
                        CurrentActivities = settings.GetActivities(),
                        CurrentProductType = settings.ProductType,
                        CurrentProductStyle = settings.ProductStyle,
                        UpdatedAt = settings.UpdatedAt ?? settings.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new StoreActivityResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving store settings",
                    Data = null
                };
            }
        }

        public async Task<StoreActivityResponse> UpdateStoreSettingsAsync(StoreActivityRequest request)
        {
            try
            {
                var settings = await _context.StoreSettings
                    .Where(s => s.IsActive && !s.IsDeleted)
                    .OrderByDescending(s => s.CreatedAt)
                    .FirstOrDefaultAsync();

                if (settings == null)
                {
                    settings = new StoreSettings
                    {
                        IsActive = true
                    };
                    _context.StoreSettings.Add(settings);
                }

                // Update settings
                settings.SetActivities(request.Activities);
                settings.ProductType = request.ProductType;
                settings.ProductStyle = request.ProductStyle;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new StoreActivityResponse
                {
                    Success = true,
                    Message = "Store settings updated successfully",
                    Data = new StoreActivityInfo
                    {
                        CurrentActivities = settings.GetActivities(),
                        CurrentProductType = settings.ProductType,
                        CurrentProductStyle = settings.ProductStyle,
                        UpdatedAt = settings.UpdatedAt ?? settings.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new StoreActivityResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تحديث إعدادات المحل",
                    Data = null
                };
            }
        }

        public async Task<StoreActivityResponse> GetAvailableActivitiesAsync()
        {
            var activities = new List<string>
            {
                "Women",
                "Men", 
                "Kids"
            };

            return new StoreActivityResponse
            {
                Success = true,
                Message = "تم جلب الأنشطة المتاحة بنجاح",
                Data = new StoreActivityInfo
                {
                    CurrentActivities = activities
                }
            };
        }

        public async Task<StoreActivityResponse> GetAvailableProductTypesAsync()
        {
            var productTypes = new List<string>
            {
                "All",
                "Dresses",
                "Tops",
                "Bottoms",
                "Outerwear",
                "Shoes",
                "Accessories",
                "Bags",
                "Jewelry"
            };

            return new StoreActivityResponse
            {
                Success = true,
                Message = "تم جلب أنواع المنتجات المتاحة بنجاح",
                Data = new StoreActivityInfo
                {
                    CurrentActivities = productTypes
                }
            };
        }

        public async Task<StoreActivityResponse> GetAvailableProductStylesAsync()
        {
            var productStyles = new List<string>
            {
                "All",
                "Casual",
                "Elegant",
                "Sporty",
                "Vintage",
                "Modern",
                "Classic",
                "Trendy",
                "Bohemian"
            };

            return new StoreActivityResponse
            {
                Success = true,
                Message = "تم جلب أنماط المنتجات المتاحة بنجاح",
                Data = new StoreActivityInfo
                {
                    CurrentActivities = productStyles
                }
            };
        }
    }
} 