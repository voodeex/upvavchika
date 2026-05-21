using shootandbunny.Context;
using shootandbunny.Entities;

namespace shootandbunny;

public class Core
{
    public static MyDbContext Context = new MyDbContext();
    public static User? CurrentUser { get; set; }
}
