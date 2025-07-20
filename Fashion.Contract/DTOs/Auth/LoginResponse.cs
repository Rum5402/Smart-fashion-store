namespace Fashion.Contract.DTOs.Auth
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public UserDto? User { get; set; }
        public ManagerDto? Manager { get; set; }
    }
    
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? SkinTone { get; set; }
    }
    
    public class ManagerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = "Manager";
        public string? Email { get; set; }
        public string? StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? StoreAddress { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
} 