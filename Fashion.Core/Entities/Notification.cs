using Fashion.Core.Entities;

namespace Fashion.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string Type { get; set; } = string.Empty; // FittingRoomRequest, General, etc.
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? UserId { get; set; } // المستخدم الذي أرسل الإشعار
        public int? ItemId { get; set; } // المنتج المرتبط بالإشعار
        public int? FittingRoomRequestId { get; set; } // طلب غرفة القياس المرتبط
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string? AdminResponse { get; set; } // رد الـ Admin
        public DateTime? RespondedAt { get; set; }

        // Navigation Properties
        public Item? Item { get; set; }
        public FittingRoomRequest? FittingRoomRequest { get; set; }
    }
} 