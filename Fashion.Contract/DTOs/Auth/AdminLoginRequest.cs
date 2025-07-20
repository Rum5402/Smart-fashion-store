using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class ManagerLoginRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
} 