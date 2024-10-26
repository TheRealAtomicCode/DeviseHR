namespace OP.DTO.Inbound
{
    public class CreateOperatorRequest
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PasswordHash { get; set; } = string.Empty;

        public int UserRole { get; set; }
    }
}
