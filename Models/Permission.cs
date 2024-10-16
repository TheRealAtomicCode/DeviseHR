using System;
using System.Collections.Generic;

namespace Models;

public partial class Permission
{
    public int Id { get; set; }

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

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
