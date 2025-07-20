using System.ComponentModel.DataAnnotations;

namespace Fashion.Core.Entities
{
    public class Manager : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? StoreName { get; set; }

        [MaxLength(500)]
        public string? StoreDescription { get; set; }

        [MaxLength(200)]
        public string? StoreAddress { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [MaxLength(100)]
        public string? Department { get; set; }
        
        [MaxLength(50)]
        public string? Position { get; set; }
        
        public bool IsSuperManager { get; set; } = false;
        
        public DateTime? LastLoginAt { get; set; }
        
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }
        
        // Navigation properties
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
} 