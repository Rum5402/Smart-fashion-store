using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Admin
{
    public class FittingRoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string? CurrentUser { get; set; }
        public DateTime? ReservedUntil { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateFittingRoomRequest
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;
    }

    public class UpdateFittingRoomRequest
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }

    public class AssignFittingRoomRequest
    {
        [Required]
        public int FittingRoomId { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        public DateTime? ReservedUntil { get; set; }
    }

    public class FittingRoomAvailabilityDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }

    public class FittingRoomStatusDto
    {
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public List<FittingRoomDto> Rooms { get; set; } = new();
    }


} 