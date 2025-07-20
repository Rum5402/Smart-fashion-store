namespace Fashion.Contract.DTOs.Store
{
    /// <summary>
    /// DTO for store information including brand, location, and contact details
    /// </summary>
    public class StoreInfoDto
    {
        /// <summary>
        /// Store ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Brand name (e.g., "ZARA")
        /// </summary>
        public string BrandName { get; set; } = string.Empty;
        
        /// <summary>
        /// Store name
        /// </summary>
        public string StoreName { get; set; } = string.Empty;
        
        /// <summary>
        /// Store location name (e.g., "Cairo Festival City Mall (New Cairo)")
        /// </summary>
        public string LocationName { get; set; } = string.Empty;
        
        /// <summary>
        /// Store address
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Store phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Store email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Store website
        /// </summary>
        public string Website { get; set; } = string.Empty;
        
        /// <summary>
        /// Store description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Store logo URL
        /// </summary>
        public string LogoUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Store banner image URL
        /// </summary>
        public string BannerUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Store opening hours
        /// </summary>
        public string OpeningHours { get; set; } = string.Empty;
        
        /// <summary>
        /// Store social media links
        /// </summary>
        public Dictionary<string, string> SocialMediaLinks { get; set; } = new();
        
        /// <summary>
        /// Store features and amenities
        /// </summary>
        public List<string> Features { get; set; } = new();
        
        /// <summary>
        /// Store is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Updated date
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// DTO for store location information
    /// </summary>
    public class StoreLocationDto
    {
        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; } = string.Empty;
        
        /// <summary>
        /// Full address
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; } = string.Empty;
        
        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; } = string.Empty;
        
        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Latitude coordinate
        /// </summary>
        public double? Latitude { get; set; }
        
        /// <summary>
        /// Longitude coordinate
        /// </summary>
        public double? Longitude { get; set; }
        
        /// <summary>
        /// Floor number
        /// </summary>
        public string Floor { get; set; } = string.Empty;
        
        /// <summary>
        /// Mall or building name
        /// </summary>
        public string MallName { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// DTO for store contact information
    /// </summary>
    public class StoreContactDto
    {
        /// <summary>
        /// Primary phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Secondary phone number
        /// </summary>
        public string SecondaryPhoneNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// WhatsApp number
        /// </summary>
        public string WhatsAppNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Website URL
        /// </summary>
        public string Website { get; set; } = string.Empty;
        
        /// <summary>
        /// Social media links
        /// </summary>
        public Dictionary<string, string> SocialMediaLinks { get; set; } = new();
    }
    
    /// <summary>
    /// DTO for store description and about information
    /// </summary>
    public class StoreDescriptionDto
    {
        /// <summary>
        /// Store description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// About us text
        /// </summary>
        public string AboutUs { get; set; } = string.Empty;
        
        /// <summary>
        /// Mission statement
        /// </summary>
        public string Mission { get; set; } = string.Empty;
        
        /// <summary>
        /// Vision statement
        /// </summary>
        public string Vision { get; set; } = string.Empty;
        
        /// <summary>
        /// Store values
        /// </summary>
        public List<string> Values { get; set; } = new();
        
        /// <summary>
        /// Store highlights
        /// </summary>
        public List<string> Highlights { get; set; } = new();
    }
} 