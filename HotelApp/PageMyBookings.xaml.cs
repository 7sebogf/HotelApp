using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageMyBookings : Page
    {
        public PageMyBookings() { InitializeComponent(); }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgBookings.ItemsSource = AppConnect.model0db.Bookings
                    .Where(b => b.UserId == AppConnect.CurrentUserId)
                    .OrderByDescending(b => b.Id)
                    .ToList();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка загрузки: {ex.Message}"); }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) { AppFrame.framemain.Navigate(new PageCatalog()); }
    }
}