using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Context;
using shootandbunny.Views;

namespace shootandbunny;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ApplyRoleVisibility();
        Navigate(new CatalogView());
    }

    private void ApplyRoleVisibility()
    {
        var user = Core.CurrentUser;
        if (user == null) return;
        using var db = new MyDbContext();
        user = db.Users.Include(u => u.Role).First(u => u.Id == user.Id);
        Core.CurrentUser = user;
        Author.IsVisible = user.Role.Name == "Автор";
        Admin.IsVisible = user.Role.Name == "Администратор";
        Frozen.IsVisible = user.IsFrozen;
    }

    public void Navigate(UserControl view)
    {
        MainContent.Content = view;
    }

    private void Catalog_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new CatalogView());
    }

    private void Lists_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new ReadingListsView());
    }

    private void Author_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new AuthorView());
    }

    private void Admin_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new AdminView());
    }

    private void Frozen_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new ProfileView());
    }

    private void Profile_Click(object? sender, RoutedEventArgs e)
    {
        Navigate(new ProfileView());
    }
}
