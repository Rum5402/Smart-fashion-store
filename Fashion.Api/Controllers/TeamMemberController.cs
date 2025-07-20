using Fashion.Api.Filters;
using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;
using Fashion.Service.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamMemberController : BaseController
    {
        private readonly ITeamMemberService _teamMemberService;

        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        [HttpPost("add")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<TeamMemberDto>>> AddTeamMember([FromBody] AddTeamMemberRequest request)
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<TeamMemberDto>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.AddTeamMemberAsync(managerId.Value, request);
            return Ok(response);
        }

        [HttpGet("list")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<List<TeamMemberDto>>>> GetTeamMembers()
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<List<TeamMemberDto>>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.GetTeamMembersAsync(managerId.Value);
            return Ok(response);
        }

        [HttpGet("active")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<List<TeamMemberDto>>>> GetActiveTeamMembers()
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<List<TeamMemberDto>>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.GetActiveTeamMembersAsync(managerId.Value);
            return Ok(response);
        }

        [HttpPut("deactivate/{teamMemberId}")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateTeamMember(int teamMemberId)
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.DeactivateTeamMemberAsync(managerId.Value, teamMemberId);
            return Ok(response);
        }

        [HttpPut("activate/{teamMemberId}")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateTeamMember(int teamMemberId)
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.ActivateTeamMemberAsync(managerId.Value, teamMemberId);
            return Ok(response);
        }

        [HttpPut("update/{teamMemberId}")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<TeamMemberDto>>> UpdateTeamMember(int teamMemberId, [FromBody] AddTeamMemberRequest request)
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<TeamMemberDto>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.UpdateTeamMemberAsync(managerId.Value, teamMemberId, request);
            return Ok(response);
        }

        [HttpDelete("delete/{teamMemberId}")]
        [AuthorizeRoles("Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTeamMember(int teamMemberId)
        {
            var managerId = GetCurrentUserId();
            if (!managerId.HasValue)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse("User not authenticated"));
            }
            var response = await _teamMemberService.DeleteTeamMemberAsync(managerId.Value, teamMemberId);
            return Ok(response);
        }
    }
} 