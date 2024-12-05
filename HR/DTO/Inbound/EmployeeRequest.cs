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
        public DateOnly AnnualLeaveYearStartDate { get; set; }
        public DateOnly AnnualLeaveStartDate { get; internal set; }
    }
}
