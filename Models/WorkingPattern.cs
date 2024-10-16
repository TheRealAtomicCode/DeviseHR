using System;
using System.Collections.Generic;

namespace Models;

public partial class WorkingPattern
{
    public int Id { get; set; }

    public string PatternName { get; set; } = null!;

    public int CompanyId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public TimeOnly? MondayStartTime { get; set; }

    public TimeOnly? MondayEndTime { get; set; }

    public TimeOnly? TuesdayStartTime { get; set; }

    public TimeOnly? TuesdayEndTime { get; set; }

    public TimeOnly? WednesdayStartTime { get; set; }

    public TimeOnly? WednesdayEndTime { get; set; }

    public TimeOnly? ThursdayStartTime { get; set; }

    public TimeOnly? ThursdayEndTime { get; set; }

    public TimeOnly? FridayStartTime { get; set; }

    public TimeOnly? FridayEndTime { get; set; }

    public TimeOnly? SaturdayStartTime { get; set; }

    public TimeOnly? SaturdayEndTime { get; set; }

    public TimeOnly? SundayStartTime { get; set; }

    public TimeOnly? SundayEndTime { get; set; }

    public virtual Company Company { get; set; } = null!;
}
