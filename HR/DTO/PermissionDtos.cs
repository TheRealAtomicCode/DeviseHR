﻿namespace HR.DTO
{

    public class AddPermissionRequest
    {
        public string PermissionName { get; set; } = null!;
        public bool EnableAddEmployees { get; set; }
        public bool EnableTerminateEmployees { get; set; }
        public bool EnableDeleteEmployee { get; set; }
        public bool EnableCreatePattern { get; set; }
        public bool EnableApproveAbsence { get; set; }
        public bool EnableAddManditoryLeave { get; set; }
        public bool EnableAddLateness { get; set; }
        public bool EnableCreateRotas { get; set; }
        public bool EnableViewEmployeeNotifications { get; set; }
        public bool EnableViewEmployeePayroll { get; set; }
        public bool EnableViewEmployeeSensitiveInformation { get; set; }
    }

    public class EditPermissionRequest
    {
        public string PermissionName { get; set; } = null!;
        public bool EnableAddEmployees { get; set; }
        public bool EnableTerminateEmployees { get; set; }
        public bool EnableDeleteEmployee { get; set; }
        public bool EnableCreatePattern { get; set; }
        public bool EnableApproveAbsence { get; set; }
        public bool EnableAddManditoryLeave { get; set; }
        public bool EnableAddLateness { get; set; }
        public bool EnableCreateRotas { get; set; }
        public bool EnableViewEmployeeNotifications { get; set; }
        public bool EnableViewEmployeePayroll { get; set; }
        public bool EnableViewEmployeeSensitiveInformation { get; set; }
    }

    public class UsersRoles
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public int? RoleId { get; set; }
    }


    public class EditSubordinatesRequest
    {
        public int ManagerId { get; set; }
        public List<int> SubordinatesToBeAdded { get; set; } = new List<int>();
        public List<int> SubordinatesToBeRemoved { get; set; } = new List<int>();
    }

    public class NonManagerUserDto
    {
        public int Id { get; set; }
        public int UserRole { get; set; }
        public int CompanyId { get; set; }
    }

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
