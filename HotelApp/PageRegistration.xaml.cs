using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageRegistration : Page
    {
        public PageRegistration() { InitializeComponent(); }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
                { MessageBox.Show("Заполните все обязательные поля!"); return; }

                if (AppConnect.model0db.Users.Any(u => u.Email == txtEmail.Text))
                { MessageBox.Show("Пользователь с таким Email уже существует!"); return; }

                var newUser = new Users
                {
                    Email = txtEmail.Text,
                    PasswordHash = txtPass.Password,
                    FullName = txtName.Text,
                    Phone = txtPhone.Text,
                    RoleId = 3 // 3 = Пользователь (Клиент)
                };

                AppConnect.model0db.Users.Add(newUser);
                AppConnect.model0db.SaveChanges();
                MessageBox.Show("Регистрация успешна!");
                AppFrame.framemain.Navigate(new PageLogin());
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) { AppFrame.framemain.Navigate(new PageLogin()); }
    }
}