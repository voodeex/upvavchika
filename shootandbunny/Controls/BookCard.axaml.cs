using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class BookCard : UserControl
{
    public Book Book { get; private set; }
    public event EventHandler<Book>? OpenRequested;
    public event EventHandler<Book>? AddToListRequested;

    public BookCard(Book book, double avgRating)
    {
        InitializeComponent();
        Book = book;
        Title.Text = book.Title;
        Author.Text = book.Author.DisplayName;
        Rating.Text = avgRating > 0 ? $"{avgRating:F1}" : "—";
        Description.Text = book.Description ?? "";
        if (!string.IsNullOrEmpty(book.CoverPath) && System.IO.File.Exists(book.CoverPath))
            ImgCover.Source = new Bitmap(book.CoverPath);
    }

    private void Open_Click(object? sender, RoutedEventArgs e)
    {
        OpenRequested?.Invoke(this, Book);
    }

    private void AddToList_Click(object? sender, RoutedEventArgs e)
    {
        AddToListRequested?.Invoke(this, Book);
    }
}
