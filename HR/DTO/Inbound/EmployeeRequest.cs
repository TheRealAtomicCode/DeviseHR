using HR.DTO.outbound;

namespace HR.DTO.Inbound
{
    public class NewEmployeeDto
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public int UserRole { get; set; }
        public int? PermissionId { get; set; }
        public bool RegisterUser { get; set; } = false;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly AnnualLeaveStartDate { get; set; }
    }

    public class EditEmployeeDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Title { get; set; }
        public string Email { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsTerminated { get; set; }
        public string? DriversLicenceNumber { get; set; }
        public DateOnly? DriversLicenceExpirationDate { get; set; }
        public string? PassportNumber { get; set; }
        public DateOnly? PassportExpirationDate { get; set; }
        public int UserRole { get; set; }
        public int? PermissionId { get; set; }
    }
}
