using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class RoleRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string? Reason { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
