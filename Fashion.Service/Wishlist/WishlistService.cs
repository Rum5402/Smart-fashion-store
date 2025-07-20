using Fashion.Contract.DTOs.Wishlist;
using Fashion.Contract.DTOs.Items;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Core.Enums;
using System.Text.Json;

namespace Fashion.Service.Wishlist
{
    public interface IWishlistService
    {
        Task<WishlistResponse> GetUserWishlistAsync(int userId);
        Task<bool> AddToWishlistAsync(int userId, int itemId);
        Task<bool> RemoveFromWishlistAsync(int userId, int itemId);
        Task<bool> IsInWishlistAsync(int userId, int itemId);
        Task<bool> RequestFromWishlistAsync(int userId, RequestFromWishlistRequest request);
    }

    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishlistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WishlistResponse> GetUserWishlistAsync(int userId)
        {
            var allWishlistItems = await _unitOfWork.Repository<WishlistItem>().GetAllAsync();
            var wishlistItems = allWishlistItems
                .Where(wi => wi.UserId == userId && !wi.IsDeleted)
                .OrderByDescending(wi => wi.AddedAt)
                .ToList();

            var itemRepo = _unitOfWork.Repository<Item>();
            var allItems = await itemRepo.GetAllAsync();

            var wishlistDtos = wishlistItems.Select(wi => new WishlistItemDto
            {
                Id = wi.Id,
                UserId = wi.UserId,
                ItemId = wi.ItemId,
                AddedAt = wi.AddedAt,
                Item = MapToItemDto(allItems.FirstOrDefault(i => i.Id == wi.ItemId) ?? new Item())
            }).ToList();

            return new WishlistResponse
            {
                Items = wishlistDtos,
                TotalCount = wishlistDtos.Count
            };
        }

        public async Task<bool> AddToWishlistAsync(int userId, int itemId)
        {
            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(itemId);
            if (item == null || !item.IsActive)
                return false;

            var allWishlistItems = await _unitOfWork.Repository<WishlistItem>().GetAllAsync();
            var exists = allWishlistItems.Any(wi => wi.UserId == userId && wi.ItemId == itemId && !wi.IsDeleted);
            if (exists)
                return true;

            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ItemId = itemId,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<WishlistItem>().AddAsync(wishlistItem);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int itemId)
        {
            var allWishlistItems = await _unitOfWork.Repository<WishlistItem>().GetAllAsync();
            var wishlistItem = allWishlistItems.FirstOrDefault(wi => wi.UserId == userId && wi.ItemId == itemId && !wi.IsDeleted);
            if (wishlistItem == null)
                return false;

            await _unitOfWork.Repository<WishlistItem>().DeleteAsync(wishlistItem);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }

        public async Task<bool> IsInWishlistAsync(int userId, int itemId)
        {
            var allWishlistItems = await _unitOfWork.Repository<WishlistItem>().GetAllAsync();
            return allWishlistItems.Any(wi => wi.UserId == userId && wi.ItemId == itemId && !wi.IsDeleted);
        }

        public async Task<bool> RequestFromWishlistAsync(int userId, RequestFromWishlistRequest request)
        {
            if (request.ItemIds == null || !request.ItemIds.Any())
                return false;

            var allWishlistItems = await _unitOfWork.Repository<WishlistItem>().GetAllAsync();
            var selectedWishlistItems = allWishlistItems
                .Where(wi => wi.UserId == userId && request.ItemIds.Contains(wi.ItemId) && !wi.IsDeleted)
                .ToList();

            if (!selectedWishlistItems.Any())
                return false;

            foreach (var wishlistItem in selectedWishlistItems)
            {
                var fittingRoomRequest = new FittingRoomRequest
                {
                    UserId = userId,
                    ItemId = wishlistItem.ItemId,
                    Status = FittingRoomStatus.NewRequest
                    // CreatedAt will be set automatically
                };
                await _unitOfWork.Repository<FittingRoomRequest>().AddAsync(fittingRoomRequest);
            }

            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        private ItemDto MapToItemDto(Item item)
        {
            if (item == null || item.Id == 0) return new ItemDto();
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                OriginalPrice = item.OriginalPrice,
                Category = item.Category,
                Style = item.Style,
                ProductType = item.ProductType,
                StoreActivity = item.StoreActivity,
                FabricType = item.FabricType,
                SubCategory = item.SubCategory,
                BrandName = item.BrandName,
                PrimaryColor = item.PrimaryColor,
                AvailableSizes = JsonSerializer.Deserialize<List<string>>(item.AvailableSizes) ?? new(),
                AvailableColors = JsonSerializer.Deserialize<List<string>>(item.AvailableColors) ?? new(),
                ImageUrls = JsonSerializer.Deserialize<List<string>>(item.ImageUrls) ?? new(),
                IsNewCollection = item.IsNewCollection,
                IsBestSeller = item.IsBestSeller,
                IsOnSale = item.IsOnSale,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                StoreCategoryId = item.StoreCategoryId,
                CategoryEntity = item.CategoryEntity != null ? new Fashion.Contract.DTOs.Items.CategoryDto
                {
                    Id = item.CategoryEntity.Id,
                    Name = item.CategoryEntity.Name,
                    Description = item.CategoryEntity.Description,
                    ImageUrl = item.CategoryEntity.ImageUrl
                } : null,
                CategoryName = item.CategoryEntity?.Name
            };
        }
    }
} 