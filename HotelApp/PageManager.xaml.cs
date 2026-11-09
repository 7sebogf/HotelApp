using System;
using System.Data.Entity; // ⭐ Обязательно для метода Include
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageManager : Page
    {
        private Bookings selectedBooking;

        public PageManager()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // ⭐ Include подгружает связанные таблицы (Статусы, Номера, Пользователи)
                var bookings = AppConnect.model0db.Bookings
                    .Include(b => b.BookingStatuses)
                    .Include(b => b.Rooms)
                    .Include(b => b.Users)
                    .OrderByDescending(b => b.Id)
                    .ToList();

                dgBookings.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBooking = dgBookings.SelectedItem as Bookings;

            if (selectedBooking != null)
            {
                foreach (ComboBoxItem item in cbStatus.Items)
                {
                    // ⭐ Защита от null (если статус не подгрузился)
                    string statusName = selectedBooking.BookingStatuses?.Name ?? "";

                    if (item.Content.ToString() == statusName)
                    {
                        cbStatus.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Выберите бронь из таблицы!");
                return;
            }

            try
            {
                string newStatusName = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

                // Ищем ID нового статуса в базе
                var newStatus = AppConnect.model0db.BookingStatuses
                    .FirstOrDefault(s => s.Name == newStatusName);

                if (newStatus != null)
                {
                    selectedBooking.StatusId = newStatus.Id;
                    AppConnect.model0db.SaveChanges();

                    LoadData();
                    MessageBox.Show($"Статус обновлен на '{newStatusName}'!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.framemain.Navigate(new PageLogin());
        }
    }
}