using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class AdminComplaintCard : UserControl
{
    private readonly int _complaintId;

    public event EventHandler<int>? ApproveRequested;
    public event EventHandler<int>? DismissRequested;

    public AdminComplaintCard(Complaint complaint)
    {
        InitializeComponent();
        _complaintId = complaint.Id;

        FromText.Text = $"От: {complaint.User.DisplayName}";
        TargetText.Text = complaint.BookId != null
            ? $"Книга: «{complaint.Book!.Title}»"
            : complaint.TargetUserId != null
                ? $"Пользователь: {complaint.TargetUser!.DisplayName}"
                : $"Отзыв на: «{complaint.Review!.Book.Title}»";
        ReasonText.Text = complaint.Reason;
        DateText.Text = complaint.CreatedAt.ToString("dd.MM.yyyy HH:mm");
    }

    private void Approve_Click(object? sender, RoutedEventArgs e)
    {
        ApproveRequested?.Invoke(this, _complaintId);
    }

    private void Dismiss_Click(object? sender, RoutedEventArgs e)
    {
        DismissRequested?.Invoke(this, _complaintId);
    }
}
