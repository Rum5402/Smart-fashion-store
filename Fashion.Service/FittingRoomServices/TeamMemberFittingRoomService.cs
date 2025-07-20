using Fashion.Contract.DTOs.Common;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Service.FittingRoomServices
{
    public class TeamMemberFittingRoomService : ITeamMemberFittingRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamMemberFittingRoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> GetNewRequestsAsync()
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var requests = allRequests.Where(fr => fr.Status == FittingRoomStatus.NewRequest).ToList();

                var result = requests.Select(fr => new
                {
                    fr.Id,
                    fr.Status,
                    fr.CreatedAt,
                    User = new { fr.User?.Name, fr.User?.PhoneNumber },
                    Item = new { fr.Item?.Name, fr.Item?.Price, fr.Item?.ImageUrls, Category = fr.Item?.CategoryEntity?.Name }
                }).ToList();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = "تم جلب الطلبات الجديدة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في جلب الطلبات الجديدة: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> GetCompletedRequestsAsync()
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var requests = allRequests.Where(fr => fr.Status == FittingRoomStatus.Completed).ToList();

                var result = requests.Select(fr => new
                {
                    fr.Id,
                    fr.Status,
                    fr.CreatedAt,
                    fr.HandledAt,
                    User = new { fr.User?.Name, fr.User?.PhoneNumber },
                    Item = new { fr.Item?.Name, fr.Item?.Price, fr.Item?.ImageUrls, Category = fr.Item?.CategoryEntity?.Name }
                }).ToList();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = "تم جلب الطلبات المكتملة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في جلب الطلبات المكتملة: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> GetCancelledRequestsAsync()
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var requests = allRequests.Where(fr => fr.Status == FittingRoomStatus.Cancelled).ToList();

                var result = requests.Select(fr => new
                {
                    fr.Id,
                    fr.Status,
                    fr.CreatedAt,
                    fr.HandledAt,
                    fr.StaffMessage,
                    User = new { fr.User?.Name, fr.User?.PhoneNumber },
                    Item = new { fr.Item?.Name, fr.Item?.Price, fr.Item?.ImageUrls, Category = fr.Item?.CategoryEntity?.Name }
                }).ToList();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = "تم جلب الطلبات الملغاة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في جلب الطلبات الملغاة: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> GetAllRequestsAsync()
        {
            try
            {
                var requests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();

                var result = requests.Select(fr => new
                {
                    fr.Id,
                    fr.Status,
                    fr.CreatedAt,
                    fr.HandledAt,
                    fr.StaffMessage,
                    User = new { fr.User?.Name, fr.User?.PhoneNumber },
                    Item = new { fr.Item?.Name, fr.Item?.Price, fr.Item?.ImageUrls, Category = fr.Item?.CategoryEntity?.Name }
                }).ToList();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = "تم جلب جميع الطلبات بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في جلب الطلبات: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> GetRequestDetailsAsync(int requestId)
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var request = allRequests.FirstOrDefault(fr => fr.Id == requestId);

                if (request == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "الطلب غير موجود"
                    };
                }

                var result = new
                {
                    request.Id,
                    request.Status,
                    request.CreatedAt,
                    request.HandledAt,
                    request.StaffMessage,
                    User = new { request.User?.Name, request.User?.PhoneNumber },
                    Item = new { request.Item?.Name, request.Item?.Price, request.Item?.ImageUrls, Category = request.Item?.CategoryEntity?.Name }
                };

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = "تم جلب تفاصيل الطلب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في جلب تفاصيل الطلب: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> CompleteRequestAsync(int requestId, int teamMemberId)
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var request = allRequests.FirstOrDefault(fr => fr.Id == requestId);

                if (request == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "الطلب غير موجود"
                    };
                }

                if (request.Status != FittingRoomStatus.NewRequest)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "لا يمكن إكمال طلب غير جديد"
                    };
                }

                request.Status = FittingRoomStatus.Completed;
                request.HandledByStaffId = teamMemberId;
                request.HandledAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = new { request.Id, request.Status, request.HandledAt },
                    Message = "تم إكمال الطلب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في إكمال الطلب: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> CancelRequestAsync(int requestId, int teamMemberId)
        {
            try
            {
                var allRequests = await _unitOfWork.Repository<FittingRoomRequest>().GetAllAsync();
                var request = allRequests.FirstOrDefault(fr => fr.Id == requestId);

                if (request == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "الطلب غير موجود"
                    };
                }

                if (request.Status != FittingRoomStatus.NewRequest)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "لا يمكن إلغاء طلب غير جديد"
                    };
                }

                request.Status = FittingRoomStatus.Cancelled;
                request.HandledByStaffId = teamMemberId;
                request.HandledAt = DateTime.UtcNow;
                request.StaffMessage = "تم إلغاء الطلب من قبل عضو الفريق";

                await _unitOfWork.SaveChangeAsync();

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = new { request.Id, request.Status, request.HandledAt, request.StaffMessage },
                    Message = "تم إلغاء الطلب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"فشل في إلغاء الطلب: {ex.Message}"
                };
            }
        }
    }
} 