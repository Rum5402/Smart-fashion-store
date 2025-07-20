using Fashion.Core.Enums;

namespace Fashion.Contract.DTOs.Admin
{
    public class FittingRoomRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserPhoneNumber { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemImageUrl { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public FittingRoomStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public string? StaffMessage { get; set; }
        public int? HandledByStaffId { get; set; }
        public string? HandledByStaffName { get; set; } = string.Empty;
        public DateTime? HandledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateFittingRoomRequestDto
    {
        public int ItemId { get; set; }
        public string? StaffMessage { get; set; }
    }

    public class UpdateFittingRoomRequestDto
    {
        public FittingRoomStatus Status { get; set; }
        public string? StaffMessage { get; set; }
    }

    public class FittingRoomRequestResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public FittingRoomRequestDto? Request { get; set; }
        public List<FittingRoomRequestDto>? Requests { get; set; }
    }

    public static class FittingRoomStatusExtensions
    {
        public static string GetDisplayName(this FittingRoomStatus status)
        {
            return status switch
            {
                FittingRoomStatus.NewRequest => "New Request",
                FittingRoomStatus.Completed => "Completed",
                FittingRoomStatus.Cancelled => "Cancelled",
                _ => status.ToString()
            };
        }
    }
} 