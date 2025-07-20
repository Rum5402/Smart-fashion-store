namespace Fashion.Contract.DTOs.Items
{
    /// <summary>
    /// Request model for saving a mix and match combination
    /// </summary>
    public class SaveMixMatchRequest
    {
        /// <summary>
        /// Name of the combination
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the combination
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// List of item IDs in the combination
        /// </summary>
        public List<int> ItemIds { get; set; } = new();
        
        /// <summary>
        /// Category of the combination
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// Style of the combination
        /// </summary>
        public string Style { get; set; } = string.Empty;
        
        /// <summary>
        /// Occasion for the combination
        /// </summary>
        public string Occasion { get; set; } = string.Empty;
        
        /// <summary>
        /// Season for the combination
        /// </summary>
        public string Season { get; set; } = string.Empty;
        
        /// <summary>
        /// Tags for the combination
        /// </summary>
        public List<string> Tags { get; set; } = new();
        
        /// <summary>
        /// Is this combination public
        /// </summary>
        public bool IsPublic { get; set; } = true;
        
        /// <summary>
        /// User ID who created the combination
        /// </summary>
        public int? UserId { get; set; }
    }
    
    /// <summary>
    /// Response model for mix and match combination
    /// </summary>
    public class MixMatchCombinationDto
    {
        /// <summary>
        /// Combination ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Name of the combination
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the combination
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Items in the combination
        /// </summary>
        public List<ItemDto> Items { get; set; } = new();
        
        /// <summary>
        /// Category of the combination
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// Style of the combination
        /// </summary>
        public string Style { get; set; } = string.Empty;
        
        /// <summary>
        /// Occasion for the combination
        /// </summary>
        public string Occasion { get; set; } = string.Empty;
        
        /// <summary>
        /// Season for the combination
        /// </summary>
        public string Season { get; set; } = string.Empty;
        
        /// <summary>
        /// Tags for the combination
        /// </summary>
        public List<string> Tags { get; set; } = new();
        
        /// <summary>
        /// Total price of the combination
        /// </summary>
        public decimal TotalPrice { get; set; }
        
        /// <summary>
        /// Is this combination public
        /// </summary>
        public bool IsPublic { get; set; }
        
        /// <summary>
        /// User ID who created the combination
        /// </summary>
        public int? UserId { get; set; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Updated date
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 