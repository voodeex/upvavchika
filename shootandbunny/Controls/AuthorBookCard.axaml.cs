using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class AuthorBookCard : UserControl
{
    private readonly Book _book;

    public event EventHandler<Book>? EditRequested;

    public AuthorBookCard(Book book, double avgRating)
    {
        InitializeComponent();
        _book = book;

        TitleText.Text = book.Title;
        GenresText.Text = book.Genres.Count > 0 ? string.Join(", ", book.Genres.Select(g => g.Name)) : "Без жанра";
        RatingText.Text = avgRating > 0 ? $"{avgRating:F1}" : "—";

        if (book.IsFrozen)
            FrozenBadge.IsVisible = true;

        if (!string.IsNullOrEmpty(book.CoverPath) && System.IO.File.Exists(book.CoverPath))
            CoverImage.Source = new Bitmap(book.CoverPath);
    }

    private void Edit_Click(object? sender, RoutedEventArgs e)
    {
        EditRequested?.Invoke(this, _book);
    }
}
