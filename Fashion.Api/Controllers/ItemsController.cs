using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Items;
using Fashion.Service.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("new-collection")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewCollection()
        {
            var items = await _itemService.GetNewCollectionAsync();
            return Ok(items);
        }

        [HttpGet("best-sellers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBestSellers()
        {
            var items = await _itemService.GetBestSellersAsync();
            return Ok(items);
        }

        [HttpGet("on-sale")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOnSale()
        {
            var items = await _itemService.GetOnSaleAsync();
            return Ok(items);
        }
    }
} 