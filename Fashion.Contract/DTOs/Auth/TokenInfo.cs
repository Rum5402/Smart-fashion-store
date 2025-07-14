using System.Security.Claims;

namespace Fashion.Contract.DTOs.Auth
{
    public class TokenInfo
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsValid { get; set; }
    }
} 