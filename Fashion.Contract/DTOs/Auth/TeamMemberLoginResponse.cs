namespace Fashion.Contract.DTOs.Auth
{
    public class TeamMemberLoginResponse
    {
        public string? Token { get; set; }
        public TeamMemberDto? TeamMember { get; set; }
    }
} 