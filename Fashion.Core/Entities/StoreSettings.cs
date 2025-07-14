using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Fashion.Core.Entities
{
    public class StoreSettings : BaseEntity
    {
        [Required]
        public string StoreActivities { get; set; } = string.Empty; // JSON array of activities
        
        public string? ProductType { get; set; }
        
        public string? ProductStyle { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Helper methods to work with JSON
        public List<string> GetActivities()
        {
            if (string.IsNullOrEmpty(StoreActivities))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(StoreActivities) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
        
        public void SetActivities(List<string> activities)
        {
            StoreActivities = JsonSerializer.Serialize(activities);
        }
    }
} 