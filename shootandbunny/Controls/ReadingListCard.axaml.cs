using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class ReadingListCard : UserControl
{
    private readonly ReadingList _entry;
    private readonly List<ReadingStatus> _statuses;

    public event EventHandler<Book>? BookOpenRequested;
    public event EventHandler<ReadingList>? RemoveRequested;
    public event EventHandler<(ReadingList Entry, ReadingStatus Status)>? StatusChanged;

    public ReadingListCard(ReadingList entry, List<ReadingStatus> statuses)
    {
        InitializeComponent();
        _entry = entry;
        _statuses = statuses;

        var book = entry.Book;
        TitleText.Text = book.Title;
        TitleText.PointerPressed += (_, _) => BookOpenRequested?.Invoke(this, book);
        AuthorText.Text = book.Author.DisplayName;

        double avg = book.Reviews.Where(r => !r.IsFrozen).Select(r => (double?)r.Rating).Average() ?? 0;
        RatingText.Text = avg > 0 ? $"{avg:F1}" : "—";

        if (!string.IsNullOrEmpty(book.CoverPath) && System.IO.File.Exists(book.CoverPath))
            CoverImage.Source = new Bitmap(book.CoverPath);

        StatusCombo.ItemsSource = statuses.Select(s => s.Name).ToList();
        StatusCombo.SelectedIndex = statuses.FindIndex(s => s.Id == entry.StatusId);
        StatusCombo.SelectionChanged += StatusCombo_Changed;
    }

    private void StatusCombo_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (StatusCombo.SelectedIndex < 0 || StatusCombo.SelectedIndex >= _statuses.Count) return;
        var status = _statuses[StatusCombo.SelectedIndex];
        StatusChanged?.Invoke(this, (_entry, status));
    }

    private void Remove_Click(object? sender, RoutedEventArgs e)
    {
        RemoveRequested?.Invoke(this, _entry);
    }
}
