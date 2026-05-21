using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class ProfileReviewCard : UserControl
{
    private readonly Review _review;

    public event EventHandler<Book>? BookOpenRequested;
    public event EventHandler<Review>? UnfreezeRequested;

    public ProfileReviewCard(Review review)
    {
        InitializeComponent();
        _review = review;

        BookTitleText.Text = review.Book.Title;
        BookTitleText.PointerPressed += (_, _) => BookOpenRequested?.Invoke(this, review.Book);

        RatingText.Text = review.Rating.ToString();
        DateText.Text = review.CreatedAt.ToString("dd.MM.yyyy");

        if (review.IsFrozen)
        {
            FrozenBadge.IsVisible = true;
            UnfreezeBtn.IsVisible = true;
        }

        if (!string.IsNullOrEmpty(review.Text))
        {
            ReviewBodyText.Text = review.Text;
            ReviewBodyText.IsVisible = true;
        }
    }

    private void Unfreeze_Click(object? sender, RoutedEventArgs e)
    {
        UnfreezeRequested?.Invoke(this, _review);
    }
}
