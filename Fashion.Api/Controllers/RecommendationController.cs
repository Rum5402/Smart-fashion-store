using Fashion.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RecommendationController : ControllerBase
    {
        // TODO: Inject IRecommendationService أو أي Service خاص بالـ AI Model عند توفره

        /// <summary>
        /// Get AI recommendation for the current user (Guest or Customer only).
        /// </summary>
        [HttpPost]
        [AuthorizeRoles("Guest", "Customer")]
        public async Task<IActionResult> GetRecommendation([FromBody] RecommendationRequest request)
        {
            // TODO: Replace with actual AI model call when available
            // var result = await _recommendationService.GetRecommendationAsync(request);
            // return Ok(result);

            // Placeholder response until AI integration
            return Ok(new { Message = "AI recommendation endpoint is ready. Integrate AI model here." });
        }
    }

    // DTO for recommendation request (adjust fields as needed for your AI model)
    public class RecommendationRequest
    {
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? SkinTone { get; set; }
        // أضف أي بيانات أخرى يحتاجها الموديل
    }
} 