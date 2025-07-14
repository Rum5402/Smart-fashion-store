using Fashion.Contract.DTOs.Items;
using Fashion.Core.Entities;
using Fashion.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/mix-match")]
    [Authorize]
    public class MixMatchController : ControllerBase
    {
        private readonly FashionDbContext _context;

        public MixMatchController(FashionDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<ItemDto>>> GetMixMatch([FromBody] MixMatchRequest request)
        {
            var baseItem = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == request.ItemId && i.IsActive && !i.IsDeleted);

            if (baseItem == null)
                return NotFound("Item not found");

            var mixMatchItems = await _context.Items
                .Include(i => i.CategoryEntity)
                .Where(i => i.Id != request.ItemId && 
                           i.IsActive && 
                           !i.IsDeleted &&
                           i.Category == baseItem.Category &&
                           i.Style != baseItem.Style)
                .OrderByDescending(i => i.IsNewCollection)
                .ThenByDescending(i => i.IsBestSeller)
                .Take(5)
                .ToListAsync();

            var result = mixMatchItems.Select(MapToItemDto).ToList();
            return Ok(result);
        }

        private static ItemDto MapToItemDto(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                OriginalPrice = item.OriginalPrice,
                Category = item.Category,
                Style = item.Style,
                FabricType = item.FabricType,
                SubCategory = item.SubCategory,
                AvailableSizes = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.AvailableSizes) ?? new(),
                AvailableColors = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.AvailableColors) ?? new(),
                ImageUrls = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.ImageUrls) ?? new(),
                IsNewCollection = item.IsNewCollection,
                IsBestSeller = item.IsBestSeller,
                IsOnSale = item.IsOnSale,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                CategoryEntity = item.CategoryEntity != null ? new CategoryDto
                {
                    Id = item.CategoryEntity.Id,
                    Name = item.CategoryEntity.Name,
                    Description = item.CategoryEntity.Description,
                    ImageUrl = item.CategoryEntity.ImageUrl
                } : null
            };
        }
    }

    public class MixMatchRequest
    {
        public int ItemId { get; set; }
    }
} 