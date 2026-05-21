using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class AdminUserRow : UserControl
{
    private readonly User _user;

    public event EventHandler<User>? FreezeRequested;
    public event EventHandler<int>? UnfreezeRequested;

    public AdminUserRow(User user)
    {
        InitializeComponent();
        _user = user;

        NameText.Text = $"{user.DisplayName} ({user.Login})";

        RoleBadgeAdmin.IsVisible  = user.Role.Name == "Администратор";
        RoleBadgeAuthor.IsVisible = user.Role.Name == "Автор";
        RoleBadgeReader.IsVisible = user.Role.Name == "Читатель";

        if (user.IsFrozen)
        {
            FrozenBadge.IsVisible = true;
            UnfreezeBtn.IsVisible = true;
        }
        else if (user.Role.Name != "Администратор")
        {
            FreezeBtn.Content = "Заморозить";
            FreezeBtn.IsVisible = true;
        }
    }

    private void Freeze_Click(object? sender, RoutedEventArgs e)
    {
        FreezeRequested?.Invoke(this, _user);
    }

    private void Unfreeze_Click(object? sender, RoutedEventArgs e)
    {
        UnfreezeRequested?.Invoke(this, _user.Id);
    }
}
