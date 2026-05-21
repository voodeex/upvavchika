using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void TabLogin_Click(object? sender, RoutedEventArgs e)
    {
        PanelLogin.IsVisible = true;
        PanelRegister.IsVisible = false;
        LineLogin.IsVisible = true;
        LineRegister.IsVisible = false;
    }

    private void TabRegister_Click(object? sender, RoutedEventArgs e)
    {
        PanelLogin.IsVisible = false;
        PanelRegister.IsVisible = true;
        LineLogin.IsVisible = false;
        LineRegister.IsVisible = true;
    }

    private void Login_Click(object? sender, RoutedEventArgs e)
    {
        LoginError.IsVisible = false;
        string login = LoginLogin.Text?.Trim() ?? "";
        string password = LoginPassword.Text ?? "";
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            LoginError.Text = "Заполните все поля.";
            LoginError.IsVisible = true;
            return;
        }
        var user = Core.Context.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == login);
        if (user == null || user.Password != password)
        {
            LoginError.Text = "Неверный логин или пароль.";
            LoginError.IsVisible = true;
            return;
        }
        Core.CurrentUser = user;
        NavSingle.Attach(new MainWindow());
        Close();
    }

    private void Register_Click(object? sender, RoutedEventArgs e)
    {
        RegError.IsVisible = false;
        string login = RegLogin.Text?.Trim() ?? "";
        string displayName = RegDisplayName.Text?.Trim() ?? "";
        string email = RegEmail.Text?.Trim() ?? "";
        string password = RegPassword.Text ?? "";
        string confirm = RegPasswordConfirm.Text ?? "";
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(displayName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            RegError.Text = "Заполните все поля.";
            RegError.IsVisible = true;
            return;
        }
        if (password != confirm)
        {
            RegError.Text = "Пароли не совпадают.";
            RegError.IsVisible = true;
            return;
        }
        if (password.Length < 6)
        {
            RegError.Text = "Пароль должен содержать минимум 6 символов.";
            RegError.IsVisible = true;
            return;
        }
        if (Core.Context.Users.Any(u => u.Login == login))
        {
            RegError.Text = "Пользователь с таким логином уже существует.";
            RegError.IsVisible = true;
            return;
        }
        if (Core.Context.Users.Any(u => u.Email == email))
        {
            RegError.Text = "Пользователь с таким email уже существует.";
            RegError.IsVisible = true;
            return;
        }
        var userRole = Core.Context.Roles.FirstOrDefault(r => r.Name == "Читатель");
        if (userRole == null)
        {
            RegError.Text = "Ошибка: роль 'Читатель' не найдена.";
            RegError.IsVisible = true;
            return;
        }
        var newUser = new User
        {
            Login = login,
            DisplayName = displayName,
            Email = email,
            Password = password,
            RoleId = userRole.Id,
            IsFrozen = false
        };
        Core.Context.Users.Add(newUser);
        Core.Context.SaveChanges();
        Core.CurrentUser = Core.Context.Users.Include(u => u.Role).First(u => u.Id == newUser.Id);
        NavSingle.Attach(new MainWindow());
        Close();
    }
}
