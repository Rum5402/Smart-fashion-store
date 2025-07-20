using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Auth
{
    public class AddTeamMemberRequest
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;
    }
} 