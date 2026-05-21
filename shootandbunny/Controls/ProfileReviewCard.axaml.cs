using System;
using Avalonia.Controls;
using Avalonia.Input;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class ProfileReviewCard : UserControl
{
    public event EventHandler<Book>? BookOpenRequested;

    public ProfileReviewCard(Review review)
    {
        InitializeComponent();

        BookTitleText.Text = review.Book.Title;
        BookTitleText.PointerPressed += (_, _) => BookOpenRequested?.Invoke(this, review.Book);

        RatingText.Text = review.Rating.ToString();
        DateText.Text = review.CreatedAt.ToString("dd.MM.yyyy");

        if (review.IsFrozen)
            FrozenBadge.IsVisible = true;

        if (!string.IsNullOrEmpty(review.Text))
        {
            ReviewBodyText.Text = review.Text;
            ReviewBodyText.IsVisible = true;
        }
    }
}
