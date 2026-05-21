using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public string? Text { get; set; }

    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsFrozen { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<UnfreezeRequest> UnfreezeRequests { get; set; } = new List<UnfreezeRequest>();

    public virtual User User { get; set; } = null!;
}
