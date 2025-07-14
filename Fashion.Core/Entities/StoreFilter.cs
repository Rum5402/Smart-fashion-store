using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fashion.Core.Enums;

namespace Fashion.Core.Entities
{
    public class StoreFilter : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public string Options { get; set; } = "[]"; // JSON array of filter options
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; } = 0;
        
        [Required]
        public FilterType Type { get; set; }

        [Required]
        public SelectionType SelectionType { get; set; }
        
        // Helper methods to work with JSON
        public List<string> GetOptions()
        {
            if (string.IsNullOrEmpty(Options))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(Options) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
        
        public void SetOptions(List<string> options)
        {
            Options = JsonSerializer.Serialize(options);
        }
    }
} 