using System;
using System.Collections.Generic;

namespace Models;

public partial class Hierarchy
{
    public int Id { get; set; }

    public int ManagerId { get; set; }

    public int SubordinateId { get; set; }

    public virtual Employee Manager { get; set; } = null!;

    public virtual Employee Subordinate { get; set; } = null!;
}
