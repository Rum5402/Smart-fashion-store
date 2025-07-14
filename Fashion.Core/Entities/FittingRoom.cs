using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class FittingRoom : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        [MaxLength(100)]
        public string? CurrentUser { get; set; }

        public DateTime? ReservedUntil { get; set; }
    }
} 