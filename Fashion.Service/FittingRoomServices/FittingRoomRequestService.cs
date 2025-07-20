using Fashion.Contract.DTOs.Admin;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.FittingRoomServices
{
    public class FittingRoomRequestService : IFittingRoomRequestService
    {
        private readonly FashionDbContext _context;

        public FittingRoomRequestService(FashionDbContext context)
        {
            _context = context;
        }

        public async Task<FittingRoomRequestResponse> CreateRequestAsync(int userId, CreateFittingRoomRequestDto request)
        {
            try
            {
                // Check if item exists
                var item = await _context.Items
                    .FirstOrDefaultAsync(i => i.Id == request.ItemId && i.IsActive && !i.IsDeleted);

                if (item == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Item not found"
                    };
                }

                // Check if user already has a pending request for this item
                var existingRequest = await _context.FittingRoomRequests
                    .FirstOrDefaultAsync(fr => fr.UserId == userId && 
                                             fr.ItemId == request.ItemId && 
                                             fr.Status == FittingRoomStatus.NewRequest &&
                                             !fr.IsDeleted);

                if (existingRequest != null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "You already have a pending request for this item"
                    };
                }

                var fittingRoomRequest = new FittingRoomRequest
                {
                    UserId = userId,
                    ItemId = request.ItemId,
                    Status = FittingRoomStatus.NewRequest,
                    StaffMessage = "The item will be ready in the fitting room within 2 minutes",
                    CreatedAt = DateTime.UtcNow
                };

                _context.FittingRoomRequests.Add(fittingRoomRequest);
                await _context.SaveChangesAsync();

                var createdRequest = await GetRequestByIdAsync(fittingRoomRequest.Id);

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "The item will be ready in the fitting room within 2 minutes",
                    Request = createdRequest.Request
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while creating fitting room request"
                };
            }
        }

        public async Task<FittingRoomRequestResponse> UpdateRequestAsync(int requestId, int managerId, UpdateFittingRoomRequestDto request)
        {
            try
            {
                var fittingRoomRequest = await _context.FittingRoomRequests
                    .Include(fr => fr.User)
                    .Include(fr => fr.Item)
                    .FirstOrDefaultAsync(fr => fr.Id == requestId && !fr.IsDeleted);

                if (fittingRoomRequest == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Fitting room request not found"
                    };
                }

                // Update request - removed AssignedFittingRoomId since it's automatic now
                fittingRoomRequest.Status = request.Status;
                fittingRoomRequest.StaffMessage = request.StaffMessage;
                fittingRoomRequest.HandledByStaffId = managerId;
                fittingRoomRequest.HandledAt = DateTime.UtcNow;
                fittingRoomRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var updatedRequest = await GetRequestByIdAsync(requestId);

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "Fitting room request updated successfully",
                    Request = updatedRequest.Request
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while updating fitting room request"
                };
            }
        }

        public async Task<FittingRoomRequestResponse> GetRequestByIdAsync(int requestId)
        {
            var request = await _context.FittingRoomRequests
                .Include(fr => fr.User)
                .Include(fr => fr.Item)
                .FirstOrDefaultAsync(fr => fr.Id == requestId && !fr.IsDeleted);

            if (request == null)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Fitting room request not found"
                };
            }

            var requestDto = MapToDto(request);

            return new FittingRoomRequestResponse
            {
                Success = true,
                Message = "Fitting room request retrieved successfully",
                Request = requestDto
            };
        }

        public async Task<FittingRoomRequestResponse> GetAllRequestsAsync()
        {
            var requests = await _context.FittingRoomRequests
                .Include(fr => fr.User)
                .Include(fr => fr.Item)
                .Where(fr => !fr.IsDeleted)
                .OrderByDescending(fr => fr.CreatedAt)
                .ToListAsync();

            var requestDtos = requests.Select(MapToDto).ToList();

            return new FittingRoomRequestResponse
            {
                Success = true,
                Message = "Fitting room requests retrieved successfully",
                Requests = requestDtos
            };
        }

        public async Task<FittingRoomRequestResponse> GetRequestsByStatusAsync(FittingRoomStatus status)
        {
            var requests = await _context.FittingRoomRequests
                .Include(fr => fr.User)
                .Include(fr => fr.Item)
                .Where(fr => fr.Status == status && !fr.IsDeleted)
                .OrderByDescending(fr => fr.CreatedAt)
                .ToListAsync();

            var requestDtos = requests.Select(MapToDto).ToList();

            return new FittingRoomRequestResponse
            {
                Success = true,
                Message = $"Fitting room requests with status '{status.GetDisplayName()}' retrieved successfully",
                Requests = requestDtos
            };
        }

        public async Task<FittingRoomRequestResponse> GetUserRequestsAsync(int userId)
        {
            var requests = await _context.FittingRoomRequests
                .Include(fr => fr.User)
                .Include(fr => fr.Item)
                .Where(fr => fr.UserId == userId && !fr.IsDeleted)
                .OrderByDescending(fr => fr.CreatedAt)
                .ToListAsync();

            var requestDtos = requests.Select(MapToDto).ToList();

            return new FittingRoomRequestResponse
            {
                Success = true,
                Message = "User fitting room requests retrieved successfully",
                Requests = requestDtos
            };
        }

        public async Task<FittingRoomRequestResponse> DeleteRequestAsync(int requestId, int staffId)
        {
            try
            {
                var fittingRoomRequest = await _context.FittingRoomRequests
                    .Include(fr => fr.User)
                    .Include(fr => fr.Item)
                    .FirstOrDefaultAsync(fr => fr.Id == requestId && !fr.IsDeleted);

                if (fittingRoomRequest == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Fitting room request not found"
                    };
                }

                // Soft delete the request
                fittingRoomRequest.IsDeleted = true;
                fittingRoomRequest.DeletedByStaffId = staffId;
                fittingRoomRequest.DeletedAt = DateTime.UtcNow;
                fittingRoomRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "Fitting room request deleted successfully"
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while deleting fitting room request"
                };
            }
        }

        public async Task<FittingRoomRequestResponse> CancelUserRequestAsync(int requestId, int userId)
        {
            try
            {
                var fittingRoomRequest = await _context.FittingRoomRequests
                    .Include(fr => fr.User)
                    .Include(fr => fr.Item)
                    .FirstOrDefaultAsync(fr => fr.Id == requestId && fr.UserId == userId && !fr.IsDeleted);

                if (fittingRoomRequest == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Fitting room request not found or you don't have permission to cancel it"
                    };
                }

                // Check if request can be cancelled (only new requests can be cancelled)
                if (fittingRoomRequest.Status != FittingRoomStatus.NewRequest)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Only new requests can be cancelled"
                    };
                }

                // Cancel the request
                fittingRoomRequest.Status = FittingRoomStatus.Cancelled;
                fittingRoomRequest.StaffMessage = "Request cancelled by user";
                fittingRoomRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "Fitting room request cancelled successfully"
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while cancelling fitting room request"
                };
            }
        }

        public async Task<FittingRoomRequestResponse> CompleteRequestAsync(int requestId, int staffId)
        {
            try
            {
                var fittingRoomRequest = await _context.FittingRoomRequests
                    .Include(fr => fr.User)
                    .Include(fr => fr.Item)
                    .FirstOrDefaultAsync(fr => fr.Id == requestId && !fr.IsDeleted);

                if (fittingRoomRequest == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Fitting room request not found"
                    };
                }

                // Check if request can be completed (only new requests can be completed)
                if (fittingRoomRequest.Status != FittingRoomStatus.NewRequest)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Only new requests can be completed"
                    };
                }

                // Complete the request
                fittingRoomRequest.Status = FittingRoomStatus.Completed;
                fittingRoomRequest.StaffMessage = "Request completed by staff";
                fittingRoomRequest.HandledByStaffId = staffId;
                fittingRoomRequest.HandledAt = DateTime.UtcNow;
                fittingRoomRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "Fitting room request completed successfully"
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while completing fitting room request"
                };
            }
        }

        public async Task<FittingRoomRequestResponse> CancelRequestByStaffAsync(int requestId, int staffId)
        {
            try
            {
                var fittingRoomRequest = await _context.FittingRoomRequests
                    .Include(fr => fr.User)
                    .Include(fr => fr.Item)
                    .FirstOrDefaultAsync(fr => fr.Id == requestId && !fr.IsDeleted);

                if (fittingRoomRequest == null)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Fitting room request not found"
                    };
                }

                // Check if request can be cancelled (only new requests can be cancelled)
                if (fittingRoomRequest.Status != FittingRoomStatus.NewRequest)
                {
                    return new FittingRoomRequestResponse
                    {
                        Success = false,
                        Message = "Only new requests can be cancelled"
                    };
                }

                // Cancel the request
                fittingRoomRequest.Status = FittingRoomStatus.Cancelled;
                fittingRoomRequest.StaffMessage = "Request cancelled by staff";
                fittingRoomRequest.HandledByStaffId = staffId;
                fittingRoomRequest.HandledAt = DateTime.UtcNow;
                fittingRoomRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new FittingRoomRequestResponse
                {
                    Success = true,
                    Message = "Fitting room request cancelled successfully"
                };
            }
            catch (Exception)
            {
                return new FittingRoomRequestResponse
                {
                    Success = false,
                    Message = "Error occurred while cancelling fitting room request"
                };
            }
        }

        private static FittingRoomRequestDto MapToDto(FittingRoomRequest request)
        {
            return new FittingRoomRequestDto
            {
                Id = request.Id,
                UserId = request.UserId,
                UserName = request.User?.Name ?? "Unknown User",
                UserPhoneNumber = request.User?.PhoneNumber ?? string.Empty,
                ItemId = request.ItemId,
                ItemName = request.Item?.Name ?? "Unknown Item",
                ItemImageUrl = request.Item?.ImageUrls ?? string.Empty,
                ItemPrice = request.Item?.Price ?? 0,
                Status = request.Status,
                StatusDisplayName = request.Status.GetDisplayName(),
                StaffMessage = request.StaffMessage,
                HandledByStaffId = request.HandledByStaffId,
                HandledByStaffName = string.Empty, // You might want to get this from a staff table
                HandledAt = request.HandledAt,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt
            };
        }
    }

    public interface IFittingRoomRequestService
    {
        Task<FittingRoomRequestResponse> CreateRequestAsync(int userId, CreateFittingRoomRequestDto request);
        Task<FittingRoomRequestResponse> UpdateRequestAsync(int requestId, int managerId, UpdateFittingRoomRequestDto request);
        Task<FittingRoomRequestResponse> GetRequestByIdAsync(int requestId);
        Task<FittingRoomRequestResponse> GetAllRequestsAsync();
        Task<FittingRoomRequestResponse> GetRequestsByStatusAsync(FittingRoomStatus status);
        Task<FittingRoomRequestResponse> GetUserRequestsAsync(int userId);
        Task<FittingRoomRequestResponse> DeleteRequestAsync(int requestId, int staffId);
        Task<FittingRoomRequestResponse> CancelUserRequestAsync(int requestId, int userId);
        Task<FittingRoomRequestResponse> CompleteRequestAsync(int requestId, int staffId);
        Task<FittingRoomRequestResponse> CancelRequestByStaffAsync(int requestId, int staffId);
    }
} 