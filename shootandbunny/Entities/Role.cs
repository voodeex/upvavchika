using System;
using System.Collections.Generic;

namespace shootandbunny.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RoleRequest> RoleRequests { get; set; } = new List<RoleRequest>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
