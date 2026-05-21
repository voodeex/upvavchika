using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class ReadingList
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public int StatusId { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ReadingStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
