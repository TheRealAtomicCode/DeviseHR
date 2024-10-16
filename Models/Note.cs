using System;
using System.Collections.Generic;

namespace Models;

public partial class Note
{
    public int Id { get; set; }

    public int? OperatorId { get; set; }

    public int? CompanyId { get; set; }

    public string? NoteContent { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Operator? Operator { get; set; }
}
