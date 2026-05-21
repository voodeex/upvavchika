using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class BookFormView : UserControl
{
    private readonly Book? _book;

    public BookFormView(Book? book = null)
    {
        InitializeComponent();
        _book = book;
        PageTitle.Text = book == null ? "Новая книга" : "Редактировать книгу";
        if (book != null)
            FillForm(book);
    }

    private void FillForm(Book book)
    {
        TitleBox.Text = book.Title;
        DescriptionBox.Text = book.Description ?? "";
        CoverPathBox.Text = book.CoverPath ?? "";
        ContentBox.Text = book.Content ?? "";
        using var db = new MyDbContext();
        var bookGenres = db.Books.Include(b => b.Genres).First(b => b.Id == book.Id).Genres.Select(g => g.Name).ToHashSet();
        Classics.IsChecked          = bookGenres.Contains("Классика");
        Detective.IsChecked         = bookGenres.Contains("Детектив");
        Drama.IsChecked             = bookGenres.Contains("Драма");
        HistoricalFiction.IsChecked = bookGenres.Contains("Историческая проза");
        Poetry.IsChecked            = bookGenres.Contains("Поэзия");
        Adventure.IsChecked         = bookGenres.Contains("Приключения");
        Psychological.IsChecked     = bookGenres.Contains("Психологический роман");
        Novel.IsChecked             = bookGenres.Contains("Роман");
        Satire.IsChecked            = bookGenres.Contains("Сатира");
        SciFi.IsChecked             = bookGenres.Contains("Фантастика");
    }

    private List<string> GetSelectedGenres()
    {
        var names = new List<string>();
        if (Classics.IsChecked == true)          names.Add("Классика");
        if (Detective.IsChecked == true)         names.Add("Детектив");
        if (Drama.IsChecked == true)             names.Add("Драма");
        if (HistoricalFiction.IsChecked == true) names.Add("Историческая проза");
        if (Poetry.IsChecked == true)            names.Add("Поэзия");
        if (Adventure.IsChecked == true)         names.Add("Приключения");
        if (Psychological.IsChecked == true)     names.Add("Психологический роман");
        if (Novel.IsChecked == true)             names.Add("Роман");
        if (Satire.IsChecked == true)            names.Add("Сатира");
        if (SciFi.IsChecked == true)             names.Add("Фантастика");
        return names;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        string title = TitleBox.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(title))
        {
            FormError.Text = "Введите название книги.";
            FormError.IsVisible = true;
            return;
        }

        using var db = new MyDbContext();
        var selectedNames = GetSelectedGenres();
        var genres = db.Genres.Where(g => selectedNames.Contains(g.Name)).ToList();

        if (_book == null)
        {
            var newBook = new Book
            {
                Title = title,
                Description = DescriptionBox.Text?.Trim(),
                CoverPath = string.IsNullOrEmpty(CoverPathBox.Text) ? null : CoverPathBox.Text.Trim(),
                Content = ContentBox.Text?.Trim(),
                AuthorId = Core.CurrentUser!.Id,
                IsFrozen = false,
                CreatedAt = DateTime.Now
            };
            foreach (var g in genres) newBook.Genres.Add(g);
            db.Books.Add(newBook);
        }
        else
        {
            var dbBook = db.Books.Include(b => b.Genres).First(b => b.Id == _book.Id);
            dbBook.Title = title;
            dbBook.Description = DescriptionBox.Text?.Trim();
            dbBook.CoverPath = string.IsNullOrEmpty(CoverPathBox.Text) ? null : CoverPathBox.Text.Trim();
            dbBook.Content = ContentBox.Text?.Trim();
            dbBook.Genres.Clear();
            foreach (var g in genres) dbBook.Genres.Add(g);
        }

        db.SaveChanges();
        FormError.IsVisible = false;

        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        mainWindow?.Navigate(new AuthorView());
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        mainWindow?.Navigate(new AuthorView());
    }
}
