using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Items;
using Fashion.Service.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Fashion.Api.Controllers
{
    /// <summary>
    /// Controller for Mix & Match functionality
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MixMatchController : ControllerBase
    {
        private readonly IItemService _itemService;

        public MixMatchController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get mix and match suggestions based on a base item
        /// </summary>
        [HttpGet("suggestions/{itemId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMixMatchSuggestions(int itemId)
        {
            try
            {
                var baseItem = await _itemService.GetItemByIdAsync(itemId);
                if (baseItem == null)
                    return NotFound(new { success = false, message = "Base item not found" });

                var suggestions = await _itemService.GetMixMatchSuggestionsAsync(itemId);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Mix & Match suggestions retrieved successfully",
                    data = new
                    {
                        baseItem,
                        suggestions
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving mix & match suggestions", details = ex.Message });
            }
        }

        /// <summary>
        /// Get mix and match outfits by category
        /// </summary>
        [HttpGet("outfits/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMixMatchOutfits(string category)
        {
            try
            {
                var outfits = await _itemService.GetMixMatchOutfitsAsync(category);
                
                return Ok(new 
                { 
                    success = true, 
                    message = $"Mix & Match outfits for {category} retrieved successfully",
                    data = outfits
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving mix & match outfits", details = ex.Message });
            }
        }

        /// <summary>
        /// Get mix and match outfits by style
        /// </summary>
        [HttpGet("outfits/style/{style}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMixMatchOutfitsByStyle(string style)
        {
            try
            {
                var outfits = await _itemService.GetMixMatchOutfitsByStyleAsync(style);
                
                return Ok(new 
                { 
                    success = true, 
                    message = $"Mix & Match outfits with style {style} retrieved successfully",
                    data = outfits
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving mix & match outfits by style", details = ex.Message });
            }
        }

        /// <summary>
        /// Get mix and match outfits by occasion
        /// </summary>
        [HttpGet("outfits/occasion/{occasion}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMixMatchOutfitsByOccasion(string occasion)
        {
            try
            {
                var outfits = await _itemService.GetMixMatchOutfitsByOccasionAsync(occasion);
                
                return Ok(new 
                { 
                    success = true, 
                    message = $"Mix & Match outfits for {occasion} occasion retrieved successfully",
                    data = outfits
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving mix & match outfits by occasion", details = ex.Message });
            }
        }

        /// <summary>
        /// Get trending mix and match combinations
        /// </summary>
        [HttpGet("trending")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrendingMixMatch()
        {
            try
            {
                var trendingOutfits = await _itemService.GetTrendingMixMatchAsync();
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Trending Mix & Match combinations retrieved successfully",
                    data = trendingOutfits
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving trending mix & match combinations", details = ex.Message });
            }
        }

        /// <summary>
        /// Get personalized mix and match recommendations
        /// </summary>
        [HttpGet("recommendations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPersonalizedMixMatchRecommendations([FromQuery] string? userPreferences = null)
        {
            try
            {
                var recommendations = await _itemService.GetPersonalizedMixMatchRecommendationsAsync(userPreferences);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Personalized Mix & Match recommendations retrieved successfully",
                    data = recommendations
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving personalized mix & match recommendations", details = ex.Message });
            }
        }

        /// <summary>
        /// Save a mix and match combination
        /// </summary>
        [HttpPost("save-combination")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMixMatchCombination([FromBody] SaveMixMatchRequest request)
        {
            try
            {
                var savedCombination = await _itemService.SaveMixMatchCombinationAsync(request);
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Mix & Match combination saved successfully",
                    data = savedCombination
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error saving mix & match combination", details = ex.Message });
            }
        }

        /// <summary>
        /// Get saved mix and match combinations
        /// </summary>
        [HttpGet("saved-combinations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSavedMixMatchCombinations()
        {
            try
            {
                var savedCombinations = await _itemService.GetSavedMixMatchCombinationsAsync();
                
                return Ok(new 
                { 
                    success = true, 
                    message = "Saved Mix & Match combinations retrieved successfully",
                    data = savedCombinations
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error retrieving saved mix & match combinations", details = ex.Message });
            }
        }
    }
} 