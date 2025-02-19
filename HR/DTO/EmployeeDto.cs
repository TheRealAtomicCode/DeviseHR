﻿namespace HR.DTO
{
    public class NewEmployeeRequest
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

    public class EditEmployeeRequest
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

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Title { get; set; }
        public string Email { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly AnnualLeaveStartDate { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsTerminated { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? NiNo { get; set; }
        public string? DriversLicenceNumber { get; set; }
        public DateOnly? DriversLicenceExpirationDate { get; set; }
        public string? PassportNumber { get; set; }
        public DateOnly? PassportExpirationDate { get; set; }
        public int UserRole { get; set; }
        public int? PermissionId { get; set; }
        public List<ManagerDto> Managers { get; set; } = new List<ManagerDto>();

    }

    public class ManagerDto
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class FoundEmployee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserRole { get; set; }
        public DateOnly AnnualLeaveStartDate { get; set; }
    }

    public class EmployeeWithContractDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserRole { get; set; }
        public DateOnly AnnualLeaveStartDate { get; set; }
        public List<ContractDtoMini> Contracts { get; set; } = new List<ContractDtoMini>();
    }


}
