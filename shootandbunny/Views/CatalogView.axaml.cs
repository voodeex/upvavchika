using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Controls;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class CatalogView : UserControl
{
    private List<Book> _books = new();

    public CatalogView()
    {
        InitializeComponent();
        LoadGenres();
        LoadAllBooks();
        RenderBooks(_books);
    }

    private void LoadGenres()
    {
        using var db = new MyDbContext();
        var genres = db.Genres
            .OrderBy(g => g.Name)
            .ToList();
        GenresList.Items.Clear();

        foreach (var genre in genres)
        {
            var checkBox = new CheckBox
            {
                Content = genre.Name,
                FontSize = 12
            };
            checkBox.IsCheckedChanged += Genre_Changed;
            GenresList.Items.Add(checkBox);
        }
    }

    private void LoadAllBooks()
    {
        using var db = new MyDbContext();
        _books = db.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .Include(b => b.Reviews)
            .Where(b => !b.IsFrozen)
            .ToList();
    }

    private void ApplyFilters()
    {
       
        var result = _books;
        string search = Search.Text?.Trim().ToLower() ?? "";
        if (!string.IsNullOrEmpty(search))
            result = result.Where(x =>
                x.Title.ToLower().Contains(search) ||
                x.Author.DisplayName.ToLower().Contains(search)).ToList();
        var selectedNames = GenresList.Items
            .OfType<CheckBox>()
            .Where(c => c.IsChecked == true)
            .Select(c => c.Content?.ToString() ?? "")
            .ToList();
        if (selectedNames.Count > 0)
            result = result.Where(x =>
                x.Genres.Any(g => selectedNames.Contains(g.Name))).ToList();
        result = Sort.SelectedIndex == 1
            ? result.OrderByDescending(x => x.Reviews.Where(r => !r.IsFrozen).Average(r => (double?)r.Rating) ?? 0).ToList()
            : result.OrderBy(x => x.Title).ToList();
        RenderBooks(result);
    }

    private void RenderBooks(List<Book> books)
    {
        BooksPanel.Children.Clear();
        foreach (var book in books)
        {
            double avgRating = book.Reviews.Where(r => !r.IsFrozen).Average(r => (double?)r.Rating) ?? 0;
            var card = new BookCard(book, avgRating);
            card.OpenRequested += (_, b) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                mainWindow?.Navigate(new BookView(b));
            };
            card.AddToListRequested += async (_, b) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                if (mainWindow == null) return;
                var dialog = new AddToListDialog(b);
                await dialog.ShowDialog(mainWindow);
            };
            BooksPanel.Children.Add(card);
        }
    }

    private void Search_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void Sort_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void Genre_Changed(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyFilters();
    }
}
