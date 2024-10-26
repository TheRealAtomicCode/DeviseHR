namespace HR.DTO.Outbound
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public bool IsTerminated { get; set; }
        public bool IsVerified { get; set; }
        public int UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Jwt { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }


}

