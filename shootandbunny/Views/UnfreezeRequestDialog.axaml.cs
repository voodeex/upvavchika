using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class UnfreezeRequestDialog : Window
{
    private readonly int _userId;

    public UnfreezeRequestDialog(int userId)
    {
        InitializeComponent();
        _userId = userId;
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
