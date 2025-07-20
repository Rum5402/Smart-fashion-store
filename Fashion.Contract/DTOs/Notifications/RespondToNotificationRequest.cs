using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Notifications
{
    public class RespondToNotificationRequest
    {
        [Required]
        [MaxLength(500)]
        public string Response { get; set; } = string.Empty;
    }
} 