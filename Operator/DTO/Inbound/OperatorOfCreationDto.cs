namespace OP.DTO.Inbound
{
    public class OperatorOfCreationDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PasswordHash { get; set; }

        public int UserRole { get; set; }

        public int AddedBy { get; set; }
    }
}
