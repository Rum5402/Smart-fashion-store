using Fashion.Contract.DTOs.Admin;

namespace Fashion.Service.Admin
{
    public interface IStoreManagementService
    {
        Task<StoreActivityResponse> GetStoreSettingsAsync();
        Task<StoreActivityResponse> UpdateStoreSettingsAsync(StoreActivityRequest request);
        Task<StoreActivityResponse> GetAvailableActivitiesAsync();
        Task<StoreActivityResponse> GetAvailableProductTypesAsync();
        Task<StoreActivityResponse> GetAvailableProductStylesAsync();
    }
} 