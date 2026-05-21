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

    public BookFormView() : this(null)
    {
    }

    public BookFormView(Book? book)
    {
        InitializeComponent();
        _book = book;
        LoadGenres();
        PageTitle.Text = book == null ? "Новая книга" : "Редактировать книгу";
        if (book != null)
            FillForm(book);
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
            GenresList.Items.Add(checkBox);
        }
    }

    private void FillForm(Book book)
    {
        TitleBox.Text = book.Title;
        DescriptionBox.Text = book.Description ?? "";
        CoverPathBox.Text = book.CoverPath ?? "";
        ContentBox.Text = book.Content ?? "";
        using var db = new MyDbContext();
        var bookGenres = db.Books.Include(b => b.Genres).First(b => b.Id == book.Id).Genres.Select(g => g.Id).ToList();
        foreach (var item in GenresList.Items.OfType<CheckBox>())
        {
            var genre = db.Genres.FirstOrDefault(g => g.Name == item.Content!.ToString());
            if (genre != null)
                item.IsChecked = bookGenres.Contains(genre.Id);
        }
    }

    private List<string> GetSelectedGenres()
    {
        return GenresList.Items
            .OfType<CheckBox>()
            .Where(c => c.IsChecked == true)
            .Select(c => c.Content?.ToString() ?? "")
            .ToList();
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
