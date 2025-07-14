using Fashion.Core.Enums;
using Fashion.Contract.DTOs.Items;

namespace Fashion.Contract.DTOs.Wishlist
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime AddedAt { get; set; }
        public ItemDto Item { get; set; } = null!;
    }
    
    public class AddToWishlistRequest
    {
        public int ItemId { get; set; }
    }
    
    public class RemoveFromWishlistRequest
    {
        public int ItemId { get; set; }
    }
    
    public class WishlistResponse
    {
        public List<WishlistItemDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
    
    public class RequestFromWishlistRequest
    {
        public List<int> ItemIds { get; set; } = new();
        public string? Notes { get; set; }
    }
} 