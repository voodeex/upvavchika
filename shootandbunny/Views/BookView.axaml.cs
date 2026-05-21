using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Controls;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class BookView : UserControl
{
    private Book _book = null!;
    private bool _isAdmin;
    private enum ComplaintTarget { Book, Author, Review }
    private ComplaintTarget _complaintTarget;
    private Review? _complaintReview;

    public BookView(Book book)
    {
        InitializeComponent();
        _isAdmin = Core.CurrentUser?.Role.Name == "Администратор";
        LoadBook(book.Id);
        LoadBookInfo();
        RenderReviews();
    }

    private void LoadBook(int bookId)
    {
        using var db = new MyDbContext();
        _book = db.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .Include(b => b.Reviews).ThenInclude(r => r.User)
            .First(b => b.Id == bookId);
    }

    private void UpdateRating()
    {
        double avgRating = _book.Reviews.Where(r => !r.IsFrozen).Select(r => (double?)r.Rating).Average() ?? 0;
        Rating.Text = avgRating > 0 ? $"{avgRating:F1}" : "—";
    }

    private void LoadBookInfo()
    {
        Title.Text = _book.Title;
        Description.Text = _book.Description ?? "";
        Author.Text = _book.Author.DisplayName;
        Genres.Text = string.Join(", ", _book.Genres.Select(g => g.Name));
        UpdateRating();
        if (!string.IsNullOrEmpty(_book.CoverPath) && System.IO.File.Exists(_book.CoverPath))
            ImgCover.Source = new Bitmap(_book.CoverPath);
        FreezeBook.IsVisible = _isAdmin;
        FreezeBook.Content = _book.IsFrozen ? "Разморозить книгу" : "Заморозить книгу";
        bool canInteract = Core.CurrentUser != null && !Core.CurrentUser.IsFrozen;
        ShowReviewForm.IsVisible = canInteract;
        ComplainBook.IsVisible = canInteract;
        ComplainAuthor.IsVisible = canInteract;
    }

    private void RenderReviews()
    {
        ReviewsPanel.Children.Clear();
        var reviews = _book.Reviews
            .Where(r => _isAdmin || !r.IsFrozen)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
        NoReviewsText.IsVisible = reviews.Count == 0;
        if (reviews.Count == 0) return;
        bool canInteract = Core.CurrentUser != null && !Core.CurrentUser.IsFrozen;
        foreach (var review in reviews)
        {
            bool isMine = Core.CurrentUser?.Id == review.UserId;
            bool canComplain = canInteract && !isMine && !_isAdmin;
            var card = new ReviewCard(review, _isAdmin, canComplain);
            card.ComplainRequested += (_, r) =>
            {
                _complaintTarget = ComplaintTarget.Review;
                _complaintReview = r;
                ComplaintTitle.Text = "Жалоба на отзыв";
                ComplaintReason.Text = "";
                ComplaintError.IsVisible = false;
                ComplaintPanel.IsVisible = true;
            };
            card.FreezeRequested += (_, r) => FreezeReview(r);
            ReviewsPanel.Children.Add(card);
        }
    }

    private void FreezeReview(Review review)
    {
        using var db = new MyDbContext();
        var entry = db.Reviews.First(x => x.Id == review.Id);
        entry.IsFrozen = !entry.IsFrozen;
        db.SaveChanges();
        LoadBook(_book.Id);
        RenderReviews();
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
        mainWindow?.Navigate(new CatalogView());
    }

    private void Read_Click(object? sender, RoutedEventArgs e)
    {
        BookContent.Text = string.IsNullOrEmpty(_book.Content) ? "Текст книги недоступен." : _book.Content;
        ReaderPanel.IsVisible = !ReaderPanel.IsVisible;
    }

    private void CloseReader_Click(object? sender, RoutedEventArgs e)
    {
        ReaderPanel.IsVisible = false;
    }

    private void ComplainBook_Click(object? sender, RoutedEventArgs e)
    {
        _complaintTarget = ComplaintTarget.Book;
        _complaintReview = null;
        ComplaintTitle.Text = "Жалоба на книгу";
        ComplaintReason.Text = "";
        ComplaintError.IsVisible = false;
        ComplaintPanel.IsVisible = true;
    }

    private void ComplainAuthor_Click(object? sender, RoutedEventArgs e)
    {
        _complaintTarget = ComplaintTarget.Author;
        _complaintReview = null;
        ComplaintTitle.Text = "Жалоба на автора";
        ComplaintReason.Text = "";
        ComplaintError.IsVisible = false;
        ComplaintPanel.IsVisible = true;
    }

    private void SubmitComplaint_Click(object? sender, RoutedEventArgs e)
    {
        string reason = ComplaintReason.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(reason))
        {
            ComplaintError.Text = "Введите причину жалобы.";
            ComplaintError.IsVisible = true;
            return;
        }
        using var db = new MyDbContext();
        var complaint = new Complaint
        {
            UserId = Core.CurrentUser!.Id,
            Reason = reason,
            CreatedAt = DateTime.Now,
            Status = "pending"
        };
        switch (_complaintTarget)
        {
            case ComplaintTarget.Book:   complaint.BookId = _book.Id; break;
            case ComplaintTarget.Author: complaint.TargetUserId = _book.AuthorId; break;
            case ComplaintTarget.Review: complaint.ReviewId = _complaintReview!.Id; break;
        }
        db.Complaints.Add(complaint);
        db.SaveChanges();
        ComplaintPanel.IsVisible = false;
    }

    private void CancelComplaint_Click(object? sender, RoutedEventArgs e)
    {
        ComplaintPanel.IsVisible = false;
    }

    private void FreezeBook_Click(object? sender, RoutedEventArgs e)
    {
        using var db = new MyDbContext();
        var book = db.Books.First(x => x.Id == _book.Id);
        book.IsFrozen = !book.IsFrozen;
        db.SaveChanges();
        _book.IsFrozen = book.IsFrozen;
        FreezeBook.Content = _book.IsFrozen ? "Разморозить книгу" : "Заморозить книгу";
    }

    private void ShowReviewForm_Click(object? sender, RoutedEventArgs e)
    {
        ReviewFormPanel.IsVisible = !ReviewFormPanel.IsVisible;
    }

    private void SubmitReview_Click(object? sender, RoutedEventArgs e)
    {
        if (ReviewRating.SelectedIndex < 0)
        {
            ReviewError.Text = "Выберите оценку.";
            ReviewError.IsVisible = true;
            return;
        }
        using var db = new MyDbContext();
        if (db.Reviews.Any(r => r.BookId == _book.Id && r.UserId == Core.CurrentUser!.Id))
        {
            ReviewError.Text = "Вы уже оставили отзыв на эту книгу.";
            ReviewError.IsVisible = true;
            return;
        }
        db.Reviews.Add(new Review
        {
            UserId = Core.CurrentUser!.Id,
            BookId = _book.Id,
            Rating = ReviewRating.SelectedIndex + 1,
            Text = ReviewText.Text?.Trim() ?? "",
            CreatedAt = DateTime.Now,
            IsFrozen = false
        });
        db.SaveChanges();
        LoadBook(_book.Id);
        ReviewFormPanel.IsVisible = false;
        ReviewText.Text = "";
        ReviewRating.SelectedIndex = -1;
        ReviewError.IsVisible = false;
        RenderReviews();
        UpdateRating();
    }

    private void CancelReview_Click(object? sender, RoutedEventArgs e)
    {
        ReviewFormPanel.IsVisible = false;
        ReviewError.IsVisible = false;
    }

    private void Author_PointerPressed(object? sender, PointerPressedEventArgs e) { }
}
