using System;
using System.Collections.Generic;

namespace Models;

public partial class Operator
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? ProfilePicture { get; set; }

    public bool IsTerminated { get; set; }

    public bool IsVerified { get; set; }

    public int UserRole { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<string>? RefreshTokens { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedByOprtator { get; set; }

    public string? VerficationCode { get; set; }

    public short? LoginAttempt { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public DateTime? LastActiveTime { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public string GetFullName()
            => $"{FirstName} {LastName}";
}
