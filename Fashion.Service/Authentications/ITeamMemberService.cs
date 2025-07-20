using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;

namespace Fashion.Service.Authentications
{
    public interface ITeamMemberService
    {
        Task<ApiResponse<TeamMemberLoginResponse>> LoginAsync(TeamMemberLoginRequest request);
        Task<ApiResponse<TeamMemberDto>> AddTeamMemberAsync(int managerId, AddTeamMemberRequest request);
        Task<ApiResponse<List<TeamMemberDto>>> GetTeamMembersAsync(int managerId);
        Task<ApiResponse<List<TeamMemberDto>>> GetActiveTeamMembersAsync(int managerId);
        Task<ApiResponse<bool>> DeactivateTeamMemberAsync(int managerId, int teamMemberId);
        Task<ApiResponse<bool>> ActivateTeamMemberAsync(int managerId, int teamMemberId);
        Task<ApiResponse<TeamMemberDto>> UpdateTeamMemberAsync(int managerId, int teamMemberId, AddTeamMemberRequest request);
        Task<ApiResponse<bool>> DeleteTeamMemberAsync(int managerId, int teamMemberId);
    }
} 