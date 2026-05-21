using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Context;
using shootandbunny.Controls;
using shootandbunny.Entities;

namespace shootandbunny.Views;

public partial class AdminView : UserControl
{
    private string _activeTab = "users";

    public AdminView()
    {
        InitializeComponent();
        RenderTab();
    }

    private void Users_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "users";
        UpdateTabHighlight();
        RenderTab();
    }

    private void Complaints_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "complaints";
        UpdateTabHighlight();
        RenderTab();
    }

    private void Roles_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "roles";
        UpdateTabHighlight();
        RenderTab();
    }

    private void Unfreeze_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "unfreeze";
        UpdateTabHighlight();
        RenderTab();
    }

    private void UpdateTabHighlight()
    {
        if (_activeTab == "users")
            TabUsers.Opacity = 1.0;
        else
            TabUsers.Opacity = 0.4;

        if (_activeTab == "complaints")
            TabComplaints.Opacity = 1.0;
        else
            TabComplaints.Opacity = 0.4;

        if (_activeTab == "roles")
            TabRoles.Opacity = 1.0;
        else
            TabRoles.Opacity = 0.4;

        if (_activeTab == "unfreeze")
            TabUnfreeze.Opacity = 1.0;
        else
            TabUnfreeze.Opacity = 0.4;
    }

    private void RenderTab()
    {
        EmptyText.IsVisible = false;
        ContentPanel.Children.Clear();
        if (_activeTab == "users") RenderUsers();
        if (_activeTab == "complaints") RenderComplaints();
        if (_activeTab == "roles") RenderRoleRequests();
        if (_activeTab == "unfreeze") RenderUnfreezeRequests();
    }

    private void RenderUsers()
    {
        using var db = new MyDbContext();
        var users = db.Users.Include(u => u.Role).OrderBy(u => u.DisplayName).ToList();
        foreach (var user in users)
        {
            var row = new AdminUserRow(user);
            row.FreezeRequested += async (_, u) =>
            {
                var dialog = new FreezeDialog(u.DisplayName);
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                if (mainWindow == null) return;
                await dialog.ShowDialog(mainWindow);
                if (dialog.FreezeReason == null) return;
                using var db2 = new MyDbContext();
                var dbUser = db2.Users.First(x => x.Id == u.Id);
                dbUser.IsFrozen = true;
                dbUser.FreezeReason = dialog.FreezeReason;
                db2.SaveChanges();
                RenderTab();
            };
            row.UnfreezeRequested += (_, userId) =>
            {
                using var db2 = new MyDbContext();
                var dbUser = db2.Users.First(u => u.Id == userId);
                dbUser.IsFrozen = false;
                dbUser.FreezeReason = null;
                db2.SaveChanges();
                RenderTab();
            };
            ContentPanel.Children.Add(row);
        }
    }

    private void RenderComplaints()
    {
        using var db = new MyDbContext();
        var complaints = db.Complaints
            .Include(c => c.User)
            .Include(c => c.Book)
            .Include(c => c.TargetUser)
            .Include(c => c.Review).ThenInclude(r => r!.Book)
            .Where(c => c.Status == "pending")
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        if (complaints.Count == 0) { EmptyText.Text = "Активных жалоб нет."; EmptyText.IsVisible = true; return; }
        foreach (var complaint in complaints)
        {
            var card = new AdminComplaintCard(complaint);
            card.ApproveRequested += (_, id) =>
            {
                using var db2 = new MyDbContext();
                db2.Complaints.First(c => c.Id == id).Status = "approved";
                db2.SaveChanges();
                RenderTab();
            };
            card.DismissRequested += (_, id) =>
            {
                using var db2 = new MyDbContext();
                db2.Complaints.Remove(db2.Complaints.First(c => c.Id == id));
                db2.SaveChanges();
                RenderTab();
            };
            ContentPanel.Children.Add(card);
        }
    }

    private void RenderRoleRequests()
    {
        using var db = new MyDbContext();
        var requests = db.RoleRequests
            .Include(r => r.User)
            .Include(r => r.Role)
            .Where(r => r.Status == "pending")
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        if (requests.Count == 0) { EmptyText.Text = "Заявок на роль нет."; EmptyText.IsVisible = true; return; }
        foreach (var request in requests)
        {
            var card = new AdminRoleRequestCard(request);
            card.ApproveRequested += (_, req) =>
            {
                using var db2 = new MyDbContext();
                db2.RoleRequests.First(r => r.Id == req.Id).Status = "approved";
                db2.Users.First(u => u.Id == req.UserId).RoleId = req.RoleId;
                db2.SaveChanges();
                RenderTab();
            };
            card.RejectRequested += (_, id) =>
            {
                using var db2 = new MyDbContext();
                db2.RoleRequests.First(r => r.Id == id).Status = "rejected";
                db2.SaveChanges();
                RenderTab();
            };
            ContentPanel.Children.Add(card);
        }
    }

    private void RenderUnfreezeRequests()
    {
        using var db = new MyDbContext();
        var requests = db.UnfreezeRequests
            .Include(r => r.User)
            .Include(r => r.Book)
            .Include(r => r.Review).ThenInclude(rv => rv!.Book)
            .Where(r => r.Status == "pending")
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        if (requests.Count == 0) { EmptyText.Text = "Запросов на разморозку нет."; EmptyText.IsVisible = true; return; }
        foreach (var request in requests)
        {
            var card = new AdminUnfreezeCard(request);
            card.ApproveRequested += (_, req) =>
            {
                using var db2 = new MyDbContext();
                db2.UnfreezeRequests.First(r => r.Id == req.Id).Status = "approved";
                if (req.BookId != null)
                    db2.Books.First(b => b.Id == req.BookId).IsFrozen = false;
                else if (req.ReviewId != null)
                    db2.Reviews.First(r => r.Id == req.ReviewId).IsFrozen = false;
                else
                {
                    var dbUser = db2.Users.First(u => u.Id == req.UserId);
                    dbUser.IsFrozen = false;
                    dbUser.FreezeReason = null;
                }
                db2.SaveChanges();
                RenderTab();
            };
            card.RejectRequested += (_, id) =>
            {
                using var db2 = new MyDbContext();
                db2.UnfreezeRequests.First(r => r.Id == id).Status = "rejected";
                db2.SaveChanges();
                RenderTab();
            };
            ContentPanel.Children.Add(card);
        }
    }
}
