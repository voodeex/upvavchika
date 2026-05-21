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
        LoadAllBooks();
        RenderBooks(_books);
    }

    private void LoadAllBooks()
    {
        using var db = new MyDbContext();
        bool isAdmin = Core.CurrentUser?.Role.Name == "Администратор";
        _books = db.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .Include(b => b.Reviews)
            .Where(b => isAdmin || !b.IsFrozen)
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
        var selectedNames = new List<string>();
        if (Classics.IsChecked == true) selectedNames.Add("Классика");
        if (Detective.IsChecked == true) selectedNames.Add("Детектив");
        if (Drama.IsChecked == true) selectedNames.Add("Драма");
        if (HistoricalFiction.IsChecked == true) selectedNames.Add("Историческая проза");
        if (Poetry.IsChecked == true) selectedNames.Add("Поэзия");
        if (Adventure.IsChecked == true) selectedNames.Add("Приключения");
        if (Psychological.IsChecked == true) selectedNames.Add("Психологический роман");
        if (Novel.IsChecked == true) selectedNames.Add("Роман");
        if (Satire.IsChecked == true) selectedNames.Add("Сатира");
        if (SciFi.IsChecked == true) selectedNames.Add("Фантастика");
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
