using Fashion.Contract.DTOs.Auth;
using Fashion.Core.Entities;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.Authentications
{
    public class ManagerProfileService : IManagerProfileService
    {
        private readonly FashionDbContext _context;

        public ManagerProfileService(FashionDbContext context)
        {
            _context = context;
        }

        public async Task<ManagerProfileResponse> GetManagerProfileAsync(int managerId)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == managerId && !m.IsDeleted);

            if (manager == null)
            {
                return new ManagerProfileResponse
                {
                    Success = false,
                    Message = "Manager not found"
                };
            }

            return new ManagerProfileResponse
            {
                Success = true,
                Message = "Manager profile retrieved successfully",
                Profile = new ManagerProfileDto
                {
                    Name = manager.Name,
                    PhoneNumber = manager.PhoneNumber,
                    Email = manager.Email,
                    StoreName = manager.StoreName,
                    StoreDescription = manager.StoreDescription,
                    Position = manager.Position,
                    ProfileImageUrl = manager.ProfileImageUrl,
                    CreatedAt = manager.CreatedAt,
                    UpdatedAt = manager.UpdatedAt,
                    LastLoginAt = manager.LastLoginAt
                }
            };
        }

        public async Task<ManagerProfileResponse> UpdateManagerProfileAsync(int managerId, UpdateManagerProfileRequest request)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == managerId && !m.IsDeleted);

            if (manager == null)
            {
                return new ManagerProfileResponse
                {
                    Success = false,
                    Message = "Manager not found"
                };
            }

            // Update allowed information only (cannot change phone number or ManagerId)
            manager.Name = request.Name;
            manager.Email = request.Email;
            manager.StoreName = request.StoreName;
            manager.StoreDescription = request.StoreDescription;
            manager.StoreAddress = request.StoreAddress;
            manager.Notes = request.Notes;
            manager.Department = request.Department;
            manager.Position = request.Position;
            manager.ProfileImageUrl = request.ProfileImageUrl;
            manager.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ManagerProfileResponse
            {
                Success = true,
                Message = "Manager profile updated successfully",
                Profile = new ManagerProfileDto
                {
                    Name = manager.Name,
                    PhoneNumber = manager.PhoneNumber,
                    Email = manager.Email,
                    StoreName = manager.StoreName,
                    StoreDescription = manager.StoreDescription,
                    Position = manager.Position,
                    ProfileImageUrl = manager.ProfileImageUrl
                }
            };
        }
    }

    public interface IManagerProfileService
    {
        Task<ManagerProfileResponse> GetManagerProfileAsync(int managerId);
        Task<ManagerProfileResponse> UpdateManagerProfileAsync(int managerId, UpdateManagerProfileRequest request);
    }
} 