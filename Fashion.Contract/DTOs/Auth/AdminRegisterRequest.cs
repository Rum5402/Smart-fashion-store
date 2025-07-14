using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class ManagerRegisterRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string StoreName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? StoreDescription { get; set; }

        [StringLength(200)]
        public string? StoreAddress { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Manager ID must be a positive number")]
        public int ManagerId { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
} 