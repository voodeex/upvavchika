using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class UnfreezeRequestDialog : Window
{
    private readonly int _userId;
    private readonly int? _bookId;
    private readonly int? _reviewId;

    public UnfreezeRequestDialog(int userId, int? bookId = null, int? reviewId = null)
    {
        InitializeComponent();
        _userId = userId;
        _bookId = bookId;
        _reviewId = reviewId;

        if (_bookId != null)
            InfoText.Text = "Опишите, почему считаете заморозку книги необоснованной.";
        else if (_reviewId != null)
            InfoText.Text = "Опишите, почему считаете заморозку отзыва необоснованной.";
    }

    private void Submit_Click(object? sender, RoutedEventArgs e)
    {
        string reason = ReasonBox.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(reason))
        {
            ErrorText.Text = "Введите текст обращения.";
            ErrorText.IsVisible = true;
            return;
        }
        using var db = new MyDbContext();
        db.UnfreezeRequests.Add(new UnfreezeRequest
        {
            UserId = _userId,
            BookId = _bookId,
            ReviewId = _reviewId,
            Reason = reason,
            Status = "pending",
            CreatedAt = DateTime.Now
        });
        db.SaveChanges();
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
