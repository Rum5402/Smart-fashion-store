using System.ComponentModel.DataAnnotations;
using Fashion.Core.Enums;

namespace Fashion.Core.Entities
{
    public class FittingRoomRequest : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public FittingRoomStatus Status { get; set; } = FittingRoomStatus.NewRequest;
        [MaxLength(500)]
        public string? StaffMessage { get; set; }
        public int? HandledByStaffId { get; set; }
        public DateTime? HandledAt { get; set; }
        public int? DeletedByStaffId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
        public virtual TeamMember? HandledByTeamMember { get; set; }
    }
} 