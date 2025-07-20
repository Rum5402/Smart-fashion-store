namespace Fashion.Contract.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? AdminResponse { get; set; }
        public bool IsRead { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? FittingRoomRequestId { get; set; }
        public string? FittingRoomStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
} 