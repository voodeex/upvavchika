using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverPath { get; set; }

    public string? Content { get; set; }

    public int AuthorId { get; set; }

    public bool IsFrozen { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<ReadingList> ReadingLists { get; set; } = new List<ReadingList>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UnfreezeRequest> UnfreezeRequests { get; set; } = new List<UnfreezeRequest>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
