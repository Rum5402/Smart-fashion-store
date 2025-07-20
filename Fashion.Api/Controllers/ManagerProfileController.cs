using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Auth;
using Fashion.Service.Authentications;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/manager/profile")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ManagerProfileController : ControllerBase
    {
        private readonly IManagerProfileService _managerProfileService;

        public ManagerProfileController(IManagerProfileService managerProfileService)
        {
            _managerProfileService = managerProfileService;
        }

        /// <summary>
        /// Get manager profile information
        /// </summary>
        [HttpGet]
        [AuthorizeRoles("Manager")]
        public async Task<IActionResult> GetManagerProfile()
        {
            try
            {
                // Extract manager ID from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int managerId))
                {
                    return Unauthorized(new ManagerProfileResponse
                    {
                        Success = false,
                        Message = "Invalid manager ID"
                    });
                }

                var result = await _managerProfileService.GetManagerProfileAsync(managerId);
                
                if (!result.Success)
                    return NotFound(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new ManagerProfileResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving profile information"
                });
            }
        }

        /// <summary>
        /// Update manager profile information
        /// </summary>
        [HttpPut]
        [AuthorizeRoles("Manager")]
        public async Task<IActionResult> UpdateManagerProfile([FromBody] UpdateManagerProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Extract manager ID from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int managerId))
                {
                    return Unauthorized(new ManagerProfileResponse
                    {
                        Success = false,
                        Message = "Invalid manager ID"
                    });
                }

                var result = await _managerProfileService.UpdateManagerProfileAsync(managerId, request);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new ManagerProfileResponse
                {
                    Success = false,
                    Message = "Error occurred while updating profile"
                });
            }
        }
    }
} 