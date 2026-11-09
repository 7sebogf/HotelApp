using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageProfile : Page
    {
        public PageProfile() { InitializeComponent(); }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = AppConnect.model0db.Users.Find(AppConnect.CurrentUserId);
                if (user != null)
                {
                    txtName.Text = user.FullName;
                    txtPhone.Text = user.Phone;
                    txtEmail.Text = user.Email;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка загрузки профиля: {ex.Message}"); }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = AppConnect.model0db.Users.Find(AppConnect.CurrentUserId);
                if (user != null)
                {
                    user.FullName = txtName.Text;
                    user.Phone = txtPhone.Text;
                    AppConnect.model0db.SaveChanges();
                    MessageBox.Show("Данные успешно обновлены!");
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка сохранения: {ex.Message}"); }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) { AppFrame.framemain.Navigate(new PageCatalog()); }
    }
}