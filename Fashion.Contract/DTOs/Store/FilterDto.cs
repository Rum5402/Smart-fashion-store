using Fashion.Core.Enums;

namespace Fashion.Contract.DTOs.Store
{
    public class FilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public FilterType Type { get; set; }
        public SelectionType SelectionType { get; set; }
        public List<string> Options { get; set; } = new();
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 