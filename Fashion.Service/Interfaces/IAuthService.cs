using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;

namespace Fashion.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> ExploreModeAsync(ExploreModeRequest request);
        Task<ApiResponse<LoginResponse>> GuestLoginAsync(GuestLoginRequest request);
        Task<LoginResponse> SaveProfileAsync(SaveProfileRequest request);
        Task<LoginResponse> ManagerLoginAsync(ManagerLoginRequest request);
        Task<LoginResponse> CreateManagerAsync(ManagerRegisterRequest request);
    }
} 