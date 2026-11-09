using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageLogin : Page
    {
        public PageLogin() { InitializeComponent(); }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = AppConnect.model0db.Users.FirstOrDefault(u => u.Email == txbLogin.Text && u.PasswordHash == txbPassword.Password);
                if (user == null) { MessageBox.Show("Неверный логин или пароль!"); return; }

                AppConnect.CurrentUserId = user.Id;
                AppConnect.UserName = user.FullName;

                if (user.RoleId == 1) AppFrame.framemain.Navigate(new PageAdmin());
                else if (user.RoleId == 2) AppFrame.framemain.Navigate(new PageManager());
                else AppFrame.framemain.Navigate(new PageCatalog());
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void Register_Click(object sender, RoutedEventArgs e) { AppFrame.framemain.Navigate(new PageRegistration()); }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.UserName = "Гость"; AppConnect.CurrentUserId = 0; AppConnect.UserRole = "Guest";
            AppFrame.framemain.Navigate(new PageCatalog());
        }
    }
}