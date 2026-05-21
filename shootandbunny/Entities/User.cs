using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsFrozen { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? FreezeReason { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Complaint> ComplaintTargetUsers { get; set; } = new List<Complaint>();

    public virtual ICollection<Complaint> ComplaintUsers { get; set; } = new List<Complaint>();

    public virtual ICollection<ReadingList> ReadingLists { get; set; } = new List<ReadingList>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<RoleRequest> RoleRequests { get; set; } = new List<RoleRequest>();

    public virtual ICollection<UnfreezeRequest> UnfreezeRequests { get; set; } = new List<UnfreezeRequest>();
}
