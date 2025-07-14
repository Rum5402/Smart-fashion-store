using Fashion.Contract.DTOs.Admin;

namespace Fashion.Service.Admin
{
    public interface IFittingRoomManagementService
    {
        Task<FittingRoomDto> CreateFittingRoomAsync(CreateFittingRoomRequest request);
        Task<FittingRoomDto?> UpdateFittingRoomAsync(int id, UpdateFittingRoomRequest request);
        Task<bool> DeleteFittingRoomAsync(int id);
        Task<FittingRoomDto?> GetFittingRoomByIdAsync(int id);
        Task<List<FittingRoomDto>> GetAllFittingRoomsAsync();
        Task<FittingRoomStatusDto> GetFittingRoomStatusAsync();
        Task<List<FittingRoomAvailabilityDto>> GetAvailableFittingRoomsAsync();
        Task<bool> AssignFittingRoomAsync(AssignFittingRoomRequest request);
        Task<bool> ReleaseFittingRoomAsync(int id);
        Task<bool> ToggleFittingRoomStatusAsync(int id);
    }
} 