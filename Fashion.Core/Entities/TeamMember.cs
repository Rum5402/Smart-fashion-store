using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class TeamMember : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string IDNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;
        
        [Required]
        public int ManagerId { get; set; }
        
        public int StoreId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime? LastLoginAt { get; set; }
        
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }
        
        [MaxLength(255)]
        public string? PasswordHash { get; set; }
        
        [MaxLength(50)]
        public string Role { get; set; } = "TeamMember";
        
        // Computed property for full name
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        // Navigation properties
        public virtual Manager Manager { get; set; } = null!;
        public virtual ICollection<FittingRoomRequest> HandledRequests { get; set; } = new List<FittingRoomRequest>();
    }
} 