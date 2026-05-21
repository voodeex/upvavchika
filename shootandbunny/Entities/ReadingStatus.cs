using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class ReadingStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ReadingList> ReadingLists { get; set; } = new List<ReadingList>();
}
