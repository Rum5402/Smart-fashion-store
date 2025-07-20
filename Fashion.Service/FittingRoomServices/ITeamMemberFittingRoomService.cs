using Fashion.Contract.DTOs.Common;

namespace Fashion.Service.FittingRoomServices
{
    public interface ITeamMemberFittingRoomService
    {
        Task<ApiResponse<object>> GetNewRequestsAsync();
        Task<ApiResponse<object>> GetCompletedRequestsAsync();
        Task<ApiResponse<object>> GetCancelledRequestsAsync();
        Task<ApiResponse<object>> GetAllRequestsAsync();
        Task<ApiResponse<object>> GetRequestDetailsAsync(int requestId);
        Task<ApiResponse<object>> CompleteRequestAsync(int requestId, int teamMemberId);
        Task<ApiResponse<object>> CancelRequestAsync(int requestId, int teamMemberId);
    }
} 