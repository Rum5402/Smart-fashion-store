using System.ComponentModel.DataAnnotations;

namespace Fashion.Contract.DTOs.Store
{
    public class UpdateStoreDescriptionRequest
    {
        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(2000)]
        public string? AboutUs { get; set; }

        [MaxLength(1000)]
        public string? Mission { get; set; }

        [MaxLength(1000)]
        public string? Vision { get; set; }

        public List<string>? Values { get; set; }

        public List<string>? Highlights { get; set; }
    }
} 