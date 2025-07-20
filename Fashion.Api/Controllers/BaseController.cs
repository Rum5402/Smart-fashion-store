using Fashion.Contract.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fashion.Api.Controllers
{
    /// <summary>
    /// Base controller with common functionality for all controllers
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets the current user ID from the JWT token
        /// </summary>
        /// <returns>User ID as integer, or null if not found</returns>
        protected int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        /// <summary>
        /// Gets the current user role from the JWT token
        /// </summary>
        /// <returns>User role, or null if not found</returns>
        protected string? GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        protected IActionResult SuccessResponse<T>(T data, string message = "Operation completed successfully", int? totalCount = null)
        {
            return Ok(ApiResponse<T>.SuccessResponse(data, message, totalCount));
        }

        /// <summary>
        /// Creates a successful response without data
        /// </summary>
        protected IActionResult SuccessResponse(string message = "Operation completed successfully")
        {
            return Ok(ApiResponse.SuccessResponse(message));
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        protected IActionResult ErrorResponse(string error, string? details = null, int statusCode = 500)
        {
            var response = ApiResponse.ErrorResponse(error, details);
            return StatusCode(statusCode, response);
        }

        /// <summary>
        /// Creates a not found response
        /// </summary>
        protected IActionResult NotFoundResponse(string message = "Resource not found")
        {
            return NotFound(ApiResponse.ErrorResponse(message));
        }

        /// <summary>
        /// Creates a bad request response
        /// </summary>
        protected IActionResult BadRequestResponse(string message = "Invalid request")
        {
            return BadRequest(ApiResponse.ErrorResponse(message));
        }

        /// <summary>
        /// Creates an unauthorized response
        /// </summary>
        protected IActionResult UnauthorizedResponse(string message = "Unauthorized access")
        {
            return Unauthorized(ApiResponse.ErrorResponse(message));
        }
    }
} 