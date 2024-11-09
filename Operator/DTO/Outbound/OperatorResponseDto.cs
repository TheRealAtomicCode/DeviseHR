namespace OP.DTO.Outbound
{
    public class OperatorResponseDto
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
    }
}
