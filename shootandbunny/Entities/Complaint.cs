using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class Complaint
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? BookId { get; set; }

    public int? ReviewId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? TargetUserId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book? Book { get; set; }

    public virtual Review? Review { get; set; }

    public virtual User? TargetUser { get; set; }

    public virtual User User { get; set; } = null!;
}
