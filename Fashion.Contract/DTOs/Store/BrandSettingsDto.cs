namespace Fashion.Contract.DTOs.Store
{
    public class BrandSettingsDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string? Tagline { get; set; }
        public string? LogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? AccentColor { get; set; }
        public bool IsActive { get; set; }
        public string? AboutText { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? SocialMediaLinks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 