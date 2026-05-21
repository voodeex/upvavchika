using Avalonia.Controls;
using Avalonia.Interactivity;

namespace shootandbunny.Views;

public partial class FreezeDialog : Window
{
    public string? FreezeReason { get; private set; }

    public FreezeDialog(string userName)
    {
        InitializeComponent();
        TitleText.Text = $"Заморозить: {userName}";
    }

    private void Submit_Click(object? sender, RoutedEventArgs e)
    {
        string reason = ReasonBox.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(reason))
        {
            ErrorText.Text = "Укажите причину.";
            ErrorText.IsVisible = true;
            return;
        }
        FreezeReason = reason;
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
