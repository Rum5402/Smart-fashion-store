using Fashion.Api.Filters;
using Fashion.Contract.Interface;
using Fashion.Contract.DTOs.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationHub _notificationHub;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationHub notificationHub, INotificationService notificationService)
        {
            _notificationHub = notificationHub;
            _notificationService = notificationService;
        }

        /// <summary>
        /// الحصول على إشعارات المستخدم الحالي
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { success = false, message = "User ID not found in token" });

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(new { success = true, notifications });
        }

        /// <summary>
        /// رد الأدمن على إشعار المستخدم (مثل طلب غرفة القياس)
        /// </summary>
        [HttpPost("{id}/respond")]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> RespondToNotification(int id, [FromBody] RespondToNotificationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _notificationService.RespondToNotificationAsync(id, request.Response);
            if (!success)
                return NotFound(new { success = false, message = "Notification not found" });

                    return Ok(new { success = true, message = "Response sent to user successfully" });
    }
}
} 