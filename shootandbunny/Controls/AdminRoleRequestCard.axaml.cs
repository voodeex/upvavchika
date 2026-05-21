using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using shootandbunny.Entities;

namespace shootandbunny.Controls;

public partial class AdminRoleRequestCard : UserControl
{
    private readonly RoleRequest _request;

    public event EventHandler<RoleRequest>? ApproveRequested;
    public event EventHandler<int>? RejectRequested;

    public AdminRoleRequestCard(RoleRequest request)
    {
        InitializeComponent();
        _request = request;

        UserRoleText.Text = $"{request.User.DisplayName} → {request.Role.Name}";
        DateText.Text = request.CreatedAt.ToString("dd.MM.yyyy HH:mm");

        if (!string.IsNullOrWhiteSpace(request.Reason))
        {
            ReasonText.Text = request.Reason;
            ReasonText.IsVisible = true;
        }
    }

    private void Approve_Click(object? sender, RoutedEventArgs e)
    {
        ApproveRequested?.Invoke(this, _request);
    }

    private void Reject_Click(object? sender, RoutedEventArgs e)
    {
        RejectRequested?.Invoke(this, _request.Id);
    }
}
