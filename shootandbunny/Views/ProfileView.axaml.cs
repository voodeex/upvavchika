using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Controls;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class ProfileView : UserControl
{
    private User _user = null!;

    public ProfileView()
    {
        InitializeComponent();
        LoadProfile();
    }

    private void LoadProfile()
    {
        using var db = new MyDbContext();
        _user = db.Users
            .Include(u => u.Role)
            .Include(u => u.Reviews).ThenInclude(r => r.Book)
            .Include(u => u.RoleRequests)
            .Include(u => u.UnfreezeRequests)
            .First(u => u.Id == Core.CurrentUser!.Id);

        DisplayNameText.Text = _user.DisplayName;
        LoginText.Text = _user.Login;
        EmailText.Text = _user.Email;
        DateText.Text = _user.CreatedAt.ToString("dd MMMM yyyy");

        RoleBadgeAdmin.IsVisible  = _user.Role.Name == "Администратор";
        RoleBadgeAuthor.IsVisible = _user.Role.Name == "Автор";
        RoleBadgeReader.IsVisible = _user.Role.Name == "Читатель";

        if (_user.IsFrozen)
        {
            FreezePanel.IsVisible = true;
            FreezeReasonText.Text = string.IsNullOrWhiteSpace(_user.FreezeReason)
                ? "Причина не указана."
                : _user.FreezeReason;

            var pendingUnfreeze = _user.UnfreezeRequests
                .Where(r => r.BookId == null && r.ReviewId == null)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefault();

            if (pendingUnfreeze != null)
            {
                Unfreeze.IsVisible = pendingUnfreeze.Status == "rejected";
                UnfreezeStatusText.Text = pendingUnfreeze.Status == "pending"
                    ? "Заявка на разморозку уже отправлена — ожидает рассмотрения."
                    : pendingUnfreeze.Status == "rejected"
                        ? "Предыдущая заявка была отклонена. Вы можете подать новую."
                        : "Заявка рассмотрена.";
                UnfreezeStatusText.IsVisible = true;
            }
        }

        if (_user.Role.Name == "Читатель" && !_user.IsFrozen)
        {
            RoleRequestPanel.IsVisible = true;
            var lastRequest = _user.RoleRequests.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
            if (lastRequest != null && lastRequest.Status == "pending")
            {
                RoleRequestForm.IsVisible = false;
                RoleRequestStatus.IsVisible = true;
                RoleRequestStatusText.Text = "Заявка на роль Автора уже отправлена и ожидает рассмотрения.";
            }
            else if (lastRequest != null && lastRequest.Status == "rejected")
            {
                RoleRequestStatus.IsVisible = true;
                RoleRequestStatusText.Text = "Предыдущая заявка была отклонена. Вы можете подать новую.";
            }
        }

        RenderReviews();
    }

    private void RenderReviews()
    {
        ReviewsPanel.Children.Clear();
        var reviews = _user.Reviews.OrderByDescending(r => r.CreatedAt).ToList();
        NoReviewsText.IsVisible = reviews.Count == 0;
        if (reviews.Count == 0) return;
        foreach (var review in reviews)
        {
            var card = new ProfileReviewCard(review);
            card.BookOpenRequested += (_, book) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                mainWindow?.Navigate(new BookView(book));
            };
            card.UnfreezeRequested += async (_, review) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                if (mainWindow == null) return;
                var dialog = new UnfreezeRequestDialog(_user.Id, reviewId: review.Id);
                await dialog.ShowDialog(mainWindow);
            };
            ReviewsPanel.Children.Add(card);
        }
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        mainWindow?.Navigate(new CatalogView());
    }

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {
        Core.CurrentUser = null;
        NavSingle.Attach(new LoginWindow());
    }

    private void SubmitRoleRequest_Click(object? sender, RoutedEventArgs e)
    {
        string reason = RoleRequestReason.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(reason))
        {
            RoleRequestError.Text = "Введите причину заявки.";
            RoleRequestError.IsVisible = true;
            return;
        }
        using var db = new MyDbContext();
        var authorRole = db.Roles.First(r => r.Name == "Автор");
        db.RoleRequests.Add(new RoleRequest
        {
            UserId = _user.Id,
            RoleId = authorRole.Id,
            Reason = reason,
            Status = "pending",
            CreatedAt = DateTime.Now
        });
        db.SaveChanges();
        RoleRequestForm.IsVisible = false;
        RoleRequestError.IsVisible = false;
        RoleRequestStatus.IsVisible = true;
        RoleRequestStatusText.Text = "Заявка на роль Автора отправлена и ожидает рассмотрения.";
    }

    private async void Unfreeze_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new UnfreezeRequestDialog(_user.Id);
        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        if (mainWindow != null)
            await dialog.ShowDialog(mainWindow);
        LoadProfile();
    }
}
