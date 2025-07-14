using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class ManagerLoginRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Manager ID must be a positive number")]
        public int ManagerId { get; set; }
    }
} 