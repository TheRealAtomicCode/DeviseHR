using System;
using System.Collections.Generic;

namespace Models;

public partial class Contract
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int CompanyId { get; set; }

    public int? PatternId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int ContractType { get; set; }

    public DateOnly ContractStartDate { get; set; }

    public int ContractedHoursPerWeek { get; set; }

    public int CompanyHoursPerWeek { get; set; }

    public int ContractedDaysPerWeek { get; set; }

    public int CompanyDaysPerWeek { get; set; }

    public int AverageWorkingDay { get; set; }

    public bool IsDays { get; set; }

    public int CompanyLeaveEntitlement { get; set; }

    public int ContractedLeaveEntitlement { get; set; }

    public int FirstLeaveAllowence { get; set; }

    public int NextLeaveAllowence { get; set; }

    public int? TermTimeId { get; set; }

    public int? DiscardedId { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Company Company { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
