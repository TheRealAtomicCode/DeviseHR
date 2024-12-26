using System;
using System.Collections.Generic;

namespace Models;

public partial class Absence
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int CompanyId { get; set; }

    public int ContractId { get; set; }

    public DateOnly AbsenceStartDate { get; set; }

    public DateOnly AbsenceEndDate { get; set; }

    public bool? IsFirstHalfDay { get; set; }

    public bool IsDays { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? DaysDeducted { get; set; }

    public int? HoursDeducted { get; set; }

    public int AbsenceTypeId { get; set; }

    public string? Comment { get; set; }

    public int? ApprovedBy { get; set; }

    public int AbsenceState { get; set; }

    public int? ApprovedByAdmin { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AbsenceType AbsenceType { get; set; } = null!;

    public virtual Employee? ApprovedByAdminNavigation { get; set; }

    public virtual Employee? ApprovedByNavigation { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
