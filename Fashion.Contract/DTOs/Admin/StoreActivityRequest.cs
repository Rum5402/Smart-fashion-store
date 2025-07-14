using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Admin
{
    public class StoreActivityRequest
    {
        [Required]
        public List<string> Activities { get; set; } = new();
        
        public string? ProductType { get; set; }
        
        public string? ProductStyle { get; set; }
    }
    
    public class StoreActivityResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public StoreActivityInfo? Data { get; set; }
    }
    
    public class StoreActivityInfo
    {
        public List<string> CurrentActivities { get; set; } = new();
        public string? CurrentProductType { get; set; }
        public string? CurrentProductStyle { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 