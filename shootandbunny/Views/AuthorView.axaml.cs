using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Controls;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class AuthorView : UserControl
{
    private List<Book> _books = new();

    public AuthorView()
    {
        InitializeComponent();
        LoadBooks();
    }

    private void LoadBooks()
    {
        using var db = new MyDbContext();
        _books = db.Books
            .Include(b => b.Genres)
            .Include(b => b.Reviews)
            .Where(b => b.AuthorId == Core.CurrentUser!.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();
        Render();
    }

    private void Render()
    {
        BooksPanel.Children.Clear();
        EmptyText.IsVisible = _books.Count == 0;
        if (_books.Count == 0) return;
        foreach (var book in _books)
        {
            double avgRating = book.Reviews.Where(r => !r.IsFrozen).Select(r => (double?)r.Rating).Average() ?? 0;
            var card = new AuthorBookCard(book, avgRating);
            card.EditRequested += (_, b) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                mainWindow?.Navigate(new BookFormView(b));
            };
            card.UnfreezeRequested += async (_, b) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                if (mainWindow == null) return;
                var dialog = new UnfreezeRequestDialog(Core.CurrentUser!.Id, bookId: b.Id);
                await dialog.ShowDialog(mainWindow);
            };
            BooksPanel.Children.Add(card);
        }
    }

    private void AddBook_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        mainWindow?.Navigate(new BookFormView());
    }
}
