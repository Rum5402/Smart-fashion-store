using Fashion.Contract.DTOs.Items;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using System.Text.Json;

namespace Fashion.Service.Items
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorDetectionService _colorDetectionService;

        public ItemService(IUnitOfWork unitOfWork, IColorDetectionService colorDetectionService)
        {
            _unitOfWork = unitOfWork;
            _colorDetectionService = colorDetectionService;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<Category>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var activeItems = items.Where(i => i.IsActive && !i.IsDeleted).ToList();
            
            return activeItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<Category>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || !item.IsActive || item.IsDeleted)
                return null;
                
            var categories = await categoryRepository.GetAllAsync();
            return MapToDto(item, categories);
        }

        public async Task<List<ItemDto>> GetNewCollectionAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<Category>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var newCollectionItems = items.Where(i => i.IsNewCollection && i.IsActive && !i.IsDeleted).ToList();
            
            return newCollectionItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetBestSellersAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<Category>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var bestSellerItems = items.Where(i => i.IsBestSeller && i.IsActive && !i.IsDeleted).ToList();
            
            return bestSellerItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<List<ItemDto>> GetOnSaleAsync()
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            var categoryRepository = _unitOfWork.Repository<Category>();
            
            var items = await itemRepository.GetAllAsync();
            var categories = await categoryRepository.GetAllAsync();
            
            var onSaleItems = items.Where(i => i.IsOnSale && i.IsActive && !i.IsDeleted).ToList();
            
            return onSaleItems.Select(item => MapToDto(item, categories)).ToList();
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemRequest request)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Category = request.Category,
                Style = request.Style,
                ProductType = request.ProductType,
                StoreActivity = request.StoreActivity,
                FabricType = request.FabricType,
                SubCategory = request.SubCategory,
                BrandName = request.BrandName,
                AvailableSizes = JsonSerializer.Serialize(request.AvailableSizes),
                AvailableColors = JsonSerializer.Serialize(request.AvailableColors),
                ImageUrls = JsonSerializer.Serialize(request.ImageUrls),
                IsNewCollection = request.IsNewCollection,
                IsBestSeller = request.IsBestSeller,
                IsOnSale = request.IsOnSale,
                CategoryId = request.CategoryId,
                IsActive = true
            };

            // Auto-detect primary color if images are provided
            if (request.ImageUrls.Any())
            {
                item.PrimaryColor = await _colorDetectionService.DetectPrimaryColorAsync(request.ImageUrls.First());
            }

            await itemRepository.AddAsync(item);
            await _unitOfWork.SaveChangeAsync();

            return await GetItemByIdAsync(item.Id) ?? MapToDto(item, new List<Category>());
        }

        public async Task<ItemDto?> UpdateItemAsync(int id, UpdateItemRequest request)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return null;

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.OriginalPrice = request.OriginalPrice;
            item.Category = request.Category;
            item.Style = request.Style;
            item.ProductType = request.ProductType;
            item.StoreActivity = request.StoreActivity;
            item.FabricType = request.FabricType;
            item.SubCategory = request.SubCategory;
            item.BrandName = request.BrandName;
            item.AvailableSizes = JsonSerializer.Serialize(request.AvailableSizes);
            item.AvailableColors = JsonSerializer.Serialize(request.AvailableColors);
            item.ImageUrls = JsonSerializer.Serialize(request.ImageUrls);
            item.IsNewCollection = request.IsNewCollection;
            item.IsBestSeller = request.IsBestSeller;
            item.IsOnSale = request.IsOnSale;
            item.IsActive = request.IsActive;
            item.CategoryId = request.CategoryId;
            item.UpdatedAt = DateTime.UtcNow;

            // Re-detect primary color if images changed
            if (request.ImageUrls.Any() && request.ImageUrls != JsonSerializer.Deserialize<List<string>>(item.ImageUrls))
            {
                item.PrimaryColor = await _colorDetectionService.DetectPrimaryColorAsync(request.ImageUrls.First());
            }

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();

            return await GetItemByIdAsync(item.Id);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return false;

            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> ToggleItemStatusAsync(int id)
        {
            var itemRepository = _unitOfWork.Repository<Item>();
            
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null || item.IsDeleted)
                return false;

            item.IsActive = !item.IsActive;
            item.UpdatedAt = DateTime.UtcNow;

            await itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        private static ItemDto MapToDto(Item item, IEnumerable<Category> categories)
        {
            var category = item.CategoryId.HasValue 
                ? categories.FirstOrDefault(c => c.Id == item.CategoryId.Value) 
                : null;
                
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
                CategoryId = item.CategoryId,
                CategoryName = category?.Name,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
        }
    }

    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<List<ItemDto>> GetNewCollectionAsync();
        Task<List<ItemDto>> GetBestSellersAsync();
        Task<List<ItemDto>> GetOnSaleAsync();
        Task<ItemDto> CreateItemAsync(CreateItemRequest request);
        Task<ItemDto?> UpdateItemAsync(int id, UpdateItemRequest request);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> ToggleItemStatusAsync(int id);
    }
} 