using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class ReviewCard : UserControl
{
    private readonly Review _review;

    public event EventHandler<Review>? ComplainRequested;
    public event EventHandler<Review>? FreezeRequested;

    public ReviewCard(Review review, bool isAdmin, bool canComplain)
    {
        InitializeComponent();
        _review = review;

        UserNameText.Text = review.User.DisplayName;
        DateText.Text = review.CreatedAt.ToString("dd.MM.yyyy");
        RatingText.Text = review.Rating.ToString();

        if (review.IsFrozen)
            FrozenBadge.IsVisible = true;

        if (!string.IsNullOrEmpty(review.Text))
        {
            ReviewBodyText.Text = review.Text;
            ReviewBodyText.IsVisible = true;
        }

        if (canComplain)
            ComplainBtn.IsVisible = true;

        if (isAdmin)
        {
            FreezeBtnFreeze.IsVisible   = !review.IsFrozen;
            FreezeBtnUnfreeze.IsVisible = review.IsFrozen;
        }

        ButtonsPanel.IsVisible = canComplain || isAdmin;
    }

    private void Complain_Click(object? sender, RoutedEventArgs e)
    {
        ComplainRequested?.Invoke(this, _review);
    }

    private void Freeze_Click(object? sender, RoutedEventArgs e)
    {
        FreezeRequested?.Invoke(this, _review);
    }
}
