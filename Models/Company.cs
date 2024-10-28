using System;
using System.Collections.Generic;

namespace Models;

public partial class Company
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string LicenceNumber { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public DateOnly AnnualLeaveStartDate { get; set; }

    public string? Logo { get; set; }

    public bool EnableSemiPersonalInformation { get; set; }

    public bool EnableShowEmployees { get; set; }

    public bool EnableToil { get; set; }

    public bool EnableOvertime { get; set; }

    public bool EnableAbsenceConflictsOutsideDepartments { get; set; }

    public bool EnableCarryover { get; set; }

    public bool EnableSelfCancelLeaveRequests { get; set; }

    public bool EnableEditMyInformation { get; set; }

    public bool EnableAcceptDeclineShifts { get; set; }

    public bool EnableTakeoverShift { get; set; }

    public bool EnableBroadcastShiftSwap { get; set; }

    public bool EnableRequireTwoStageApproval { get; set; }

    public string? Lang { get; set; }

    public string? Country { get; set; }

    public int? MainContactId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsSpecialClient { get; set; }

    public int MaxEmployeesAllowed { get; set; }

    public string? SecurityQuestionOne { get; set; }

    public string? SecurityQuestionTwo { get; set; }

    public string? SecurityAnswerOne { get; set; }

    public string? SecurityAnswerTwo { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int AddedByOperator { get; set; }

    public int? UpdatedByOperator { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<WorkingPattern> WorkingPatterns { get; set; } = new List<WorkingPattern>();
}
