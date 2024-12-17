namespace HR.DTO.outbound
{
    public class SubordinateResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public int UserRole { get; set; }
       // public bool IsSubordinate { get; set; }
    }


}
