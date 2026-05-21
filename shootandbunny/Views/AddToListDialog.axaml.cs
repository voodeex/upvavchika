using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class AddToListDialog : Window
{
    private readonly int _bookId;
    private List<ReadingStatus> _statuses = new();

    public AddToListDialog(Book book)
    {
        InitializeComponent();
        _bookId = book.Id;
        BookTitleText.Text = book.Title;

        using var db = new MyDbContext();
        _statuses = db.ReadingStatuses.ToList();
        foreach (var s in _statuses)
            StatusCombo.Items.Add(new ComboBoxItem { Content = s.Name });
        if (_statuses.Count > 0)
            StatusCombo.SelectedIndex = 0;

        var existing = db.ReadingLists.FirstOrDefault(rl => rl.BookId == book.Id && rl.UserId == Core.CurrentUser!.Id);
        if (existing != null)
        {
            ErrorText.Text = "Книга уже в вашем списке.";
            ErrorText.IsVisible = true;
            Submit.IsEnabled = false;
        }
    }

    private void Submit_Click(object? sender, RoutedEventArgs e)
    {
        if (StatusCombo.SelectedIndex < 0) return;
        var status = _statuses[StatusCombo.SelectedIndex];
        using var db = new MyDbContext();
        db.ReadingLists.Add(new ReadingList
        {
            UserId = Core.CurrentUser!.Id,
            BookId = _bookId,
            StatusId = status.Id,
            AddedAt = DateTime.Now
        });
        db.SaveChanges();
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
