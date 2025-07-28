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
        // TODO: Inject IRecommendationService when available

        /// <summary>
        /// Get recommendation for the current user (Guest or Customer only).
        /// </summary>
        [HttpPost]
        [AuthorizeRoles("Guest", "Customer")]
        public IActionResult GetRecommendation([FromBody] RecommendationRequest request)
        {
            // TODO: Replace with actual recommendation service call when available
            // var result = await _recommendationService.GetRecommendationAsync(request);
            // return Ok(result);

            // Placeholder response until recommendation integration
            return Ok(new { Message = "Recommendation endpoint is ready. Integrate recommendation service here." });
        }
    }

    // DTO for recommendation request
    public class RecommendationRequest
    {
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? SkinTone { get; set; }
        // Add any other data needed for recommendations
    }
} 