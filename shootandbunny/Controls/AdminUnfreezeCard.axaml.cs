using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class AdminUnfreezeCard : UserControl
{
    private readonly UnfreezeRequest _request;

    public event EventHandler<UnfreezeRequest>? ApproveRequested;
    public event EventHandler<int>? RejectRequested;

    public AdminUnfreezeCard(UnfreezeRequest request)
    {
        InitializeComponent();
        _request = request;

        TargetText.Text = request.BookId != null
            ? $"Книга: «{request.Book!.Title}»"
            : request.ReviewId != null
                ? $"Отзыв на: «{request.Review!.Book.Title}»"
                : $"Аккаунт: {request.User.DisplayName}";
        ReasonText.Text = request.Reason;
        DateText.Text = request.CreatedAt.ToString("dd.MM.yyyy HH:mm");
    }

    private void Approve_Click(object? sender, RoutedEventArgs e)
    {
        ApproveRequested?.Invoke(this, _request);
    }

    private void Reject_Click(object? sender, RoutedEventArgs e)
    {
        RejectRequested?.Invoke(this, _request.Id);
    }
}
