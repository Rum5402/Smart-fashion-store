using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Admin;
using Fashion.Service.FittingRoomServices;
using Fashion.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/fitting-room-requests")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class FittingRoomRequestController : ControllerBase
    {
        private readonly IFittingRoomRequestService _fittingRoomRequestService;

        public FittingRoomRequestController(IFittingRoomRequestService fittingRoomRequestService)
        {
            _fittingRoomRequestService = fittingRoomRequestService;
        }

        #region Customer Endpoints

        /// <summary>
        /// Create a new fitting room request (Customer)
        /// </summary>
        [HttpPost]
        [AuthorizeRoles("Customer", "Guest", "Explore")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateFittingRoomRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Extract user ID from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid user ID"
                    });
                }

                var result = await _fittingRoomRequestService.CreateRequestAsync(userId, request);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while creating fitting room request"
                });
            }
        }

        /// <summary>
        /// Get user's fitting room requests (Customer)
        /// </summary>
        [HttpGet("my-requests")]
        [AuthorizeRoles("Customer", "Guest", "Explore")]
        public async Task<IActionResult> GetMyRequests()
        {
            try
            {
                // Extract user ID from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid user ID"
                    });
                }

                var result = await _fittingRoomRequestService.GetUserRequestsAsync(userId);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving user requests"
                });
            }
        }

        /// <summary>
        /// Cancel user's fitting room request (Customer)
        /// </summary>
        [HttpPut("cancel/{requestId}")]
        [AuthorizeRoles("Customer", "Guest", "Explore")]
        public async Task<IActionResult> CancelMyRequest(int requestId)
        {
            try
            {
                // Extract user ID from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid user ID"
                    });
                }

                var result = await _fittingRoomRequestService.CancelUserRequestAsync(requestId, userId);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while canceling fitting room request"
                });
            }
        }

        #endregion

        #region Manager & Team Member Endpoints

        /// <summary>
        /// Get all fitting room requests (Manager & Team Member)
        /// </summary>
        [HttpGet]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetAllRequests()
        {
            try
            {
                var result = await _fittingRoomRequestService.GetAllRequestsAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving fitting room requests"
                });
            }
        }

        /// <summary>
        /// Get fitting room requests by status (Manager & Team Member)
        /// </summary>
        [HttpGet("status/{status}")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetRequestsByStatus(int status)
        {
            try
            {
                var fittingRoomStatus = (FittingRoomStatus)status;
                var result = await _fittingRoomRequestService.GetRequestsByStatusAsync(fittingRoomStatus);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving fitting room requests"
                });
            }
        }

        /// <summary>
        /// Get new fitting room requests (Manager & Team Member)
        /// </summary>
        [HttpGet("new")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetNewRequests()
        {
            try
            {
                var result = await _fittingRoomRequestService.GetRequestsByStatusAsync(FittingRoomStatus.NewRequest);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving new fitting room requests"
                });
            }
        }

        /// <summary>
        /// Get completed fitting room requests (Manager & Team Member)
        /// </summary>
        [HttpGet("completed")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetCompletedRequests()
        {
            try
            {
                var result = await _fittingRoomRequestService.GetRequestsByStatusAsync(FittingRoomStatus.Completed);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving completed fitting room requests"
                });
            }
        }

        /// <summary>
        /// Get cancelled fitting room requests (Manager & Team Member)
        /// </summary>
        [HttpGet("cancelled")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetCancelledRequests()
        {
            try
            {
                var result = await _fittingRoomRequestService.GetRequestsByStatusAsync(FittingRoomStatus.Cancelled);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving cancelled fitting room requests"
                });
            }
        }

        /// <summary>
        /// Get specific fitting room request (Manager & Team Member)
        /// </summary>
        [HttpGet("{requestId}")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            try
            {
                var result = await _fittingRoomRequestService.GetRequestByIdAsync(requestId);
                
                if (!result.Success)
                    return NotFound(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while retrieving fitting room request"
                });
            }
        }

        /// <summary>
        /// Complete fitting room request (Manager & Team Member)
        /// </summary>
        [HttpPut("{requestId}/complete")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> CompleteRequest(int requestId)
        {
            try
            {
                // Extract staff ID from token
                var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(staffIdClaim, out int staffId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid staff ID"
                    });
                }

                var result = await _fittingRoomRequestService.CompleteRequestAsync(requestId, staffId);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while completing fitting room request"
                });
            }
        }

        /// <summary>
        /// Cancel fitting room request by staff (Manager & Team Member)
        /// </summary>
        [HttpPut("{requestId}/cancel-by-staff")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> CancelRequestByStaff(int requestId)
        {
            try
            {
                // Extract staff ID from token
                var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(staffIdClaim, out int staffId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid staff ID"
                    });
                }

                var result = await _fittingRoomRequestService.CancelRequestByStaffAsync(requestId, staffId);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while cancelling fitting room request"
                });
            }
        }

        /// <summary>
        /// Delete fitting room request (Manager & Team Member)
        /// </summary>
        [HttpDelete("{requestId}")]
        [AuthorizeRoles("Manager", "TeamMember")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            try
            {
                // Extract staff ID from token
                var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(staffIdClaim, out int staffId))
                {
                    return Unauthorized(new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Invalid staff ID"
                    });
                }

                var result = await _fittingRoomRequestService.DeleteRequestAsync(requestId, staffId);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while deleting fitting room request"
                });
            }
        }

        #endregion
    }
} 