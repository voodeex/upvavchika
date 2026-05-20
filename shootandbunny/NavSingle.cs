using Avalonia.Controls;

namespace shootandbunny;

public static class NavSingle
{
    public static Window? Current { get; private set; }

    public static void Attach(Window next)
    {
        next.Show();
        Current?.Close();
        Current = next;
    }
}
