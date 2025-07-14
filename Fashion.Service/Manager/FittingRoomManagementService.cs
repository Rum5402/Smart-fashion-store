using Fashion.Contract.DTOs.Admin;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.Admin
{
    public class FittingRoomManagementService : IFittingRoomManagementService
    {
        private readonly FashionDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FittingRoomManagementService(FashionDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<FittingRoomDto> CreateFittingRoomAsync(CreateFittingRoomRequest request)
        {
            var entity = new FittingRoom
            {
                RoomNumber = request.RoomNumber,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<FittingRoomDto?> UpdateFittingRoomAsync(int id, UpdateFittingRoomRequest request)
        {
            var entity = await _context.Set<FittingRoom>().FindAsync(id);
            if (entity == null) return null;
            entity.RoomNumber = request.RoomNumber;
            entity.IsAvailable = request.IsAvailable;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> DeleteFittingRoomAsync(int id)
        {
            var entity = await _context.Set<FittingRoom>().FindAsync(id);
            if (entity == null) return false;
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FittingRoomDto?> GetFittingRoomByIdAsync(int id)
        {
            var entity = await _context.Set<FittingRoom>().FindAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<List<FittingRoomDto>> GetAllFittingRoomsAsync()
        {
            var rooms = await _context.Set<FittingRoom>().OrderBy(r => r.RoomNumber).ToListAsync();
            return rooms.Select(MapToDto).ToList();
        }

        public async Task<FittingRoomStatusDto> GetFittingRoomStatusAsync()
        {
            var rooms = await _context.Set<FittingRoom>().ToListAsync();
            return new FittingRoomStatusDto
            {
                TotalRooms = rooms.Count,
                AvailableRooms = rooms.Count(r => r.IsAvailable),
                OccupiedRooms = rooms.Count(r => !r.IsAvailable),
                Rooms = rooms.Select(MapToDto).ToList()
            };
        }

        public async Task<List<FittingRoomAvailabilityDto>> GetAvailableFittingRoomsAsync()
        {
            var rooms = await _context.Set<FittingRoom>().Where(r => r.IsAvailable).ToListAsync();
            return rooms.Select(r => new FittingRoomAvailabilityDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                IsAvailable = r.IsAvailable
            }).ToList();
        }

        public async Task<bool> AssignFittingRoomAsync(AssignFittingRoomRequest request)
        {
            var room = await _context.Set<FittingRoom>().FindAsync(request.FittingRoomId);
            if (room == null || !room.IsAvailable) return false;
            room.IsAvailable = false;
            room.CurrentUser = request.UserName;
            room.ReservedUntil = request.ReservedUntil;
            room.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseFittingRoomAsync(int id)
        {
            var room = await _context.Set<FittingRoom>().FindAsync(id);
            if (room == null || room.IsAvailable) return false;
            room.IsAvailable = true;
            room.CurrentUser = null;
            room.ReservedUntil = null;
            room.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleFittingRoomStatusAsync(int id)
        {
            var room = await _context.Set<FittingRoom>().FindAsync(id);
            if (room == null) return false;
            room.IsAvailable = !room.IsAvailable;
            room.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private static FittingRoomDto MapToDto(FittingRoom entity)
        {
            return new FittingRoomDto
            {
                Id = entity.Id,
                RoomNumber = entity.RoomNumber,
                IsAvailable = entity.IsAvailable,
                CurrentUser = entity.CurrentUser,
                ReservedUntil = entity.ReservedUntil,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
} 