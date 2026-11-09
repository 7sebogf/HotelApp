using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageCatalog : Page
    {
        private bool _isFirstLoad = false;

        public PageCatalog()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isFirstLoad) return;
            _isFirstLoad = true;

            tbName.Text = AppConnect.UserName;

            if (AppConnect.CurrentUserId == 0)
            {
                btnMyBookings.Visibility = Visibility.Collapsed;
                btnProfile.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Collapsed;

                btnLogin.Visibility = Visibility.Visible;
            }

            LoadCategories();
            LoadRooms();
        }

        private void LoadCategories()
        {
            var types = AppConnect.model0db.RoomCategories.Select(t => t.Name).ToList();
            types.Insert(0, "Все категории");
            cbFilter.ItemsSource = types;
            cbFilter.SelectedIndex = 0;
        }

        private void LoadRooms()
        {
            try
            {
                var rooms = AppConnect.model0db.Rooms.ToList();

                foreach (var r in rooms)
                {
                    // Если путь начинается с диска (C:\), то используем его как есть
                    if (!string.IsNullOrEmpty(r.ImagePath) && r.ImagePath.Contains(":\\"))
                    {
                        r.ImageUrl = r.ImagePath;
                    }
                    // Иначе (если это просто "room1.jpg") ищем в локальной папке
                    else
                    {
                        r.ImageUrl = $"/Images/{r.ImagePath}";
                    }
                }

                // Поиск
                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    string s = tbSearch.Text.ToLower();
                    rooms = rooms.Where(r => r.Number.ToLower().Contains(s)).ToList();
                }

                // Фильтр по категории
                if (cbFilter.SelectedItem != null && cbFilter.SelectedItem.ToString() != "Все категории")
                {
                    string cat = cbFilter.SelectedItem.ToString();
                    var category = AppConnect.model0db.RoomCategories.FirstOrDefault(c => c.Name == cat);
                    if (category != null) { rooms = rooms.Where(r => r.CategoryId == category.Id).ToList(); }
                }

                ListRooms.ItemsSource = rooms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFirstLoad) LoadRooms();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isFirstLoad) LoadRooms();
        }

        private void BookRoom_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.CurrentUserId == 0)
            {
                MessageBox.Show("Для бронирования необходимо войти в систему!");
                return;
            }

            var btn = sender as Button;
            var room = AppConnect.model0db.Rooms.Find(Convert.ToInt32(btn.Tag));

            new BookingWindow(room).ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.framemain.Navigate(new PageLogin());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.framemain.Navigate(new PageProfile());
        }

        private void btnMyBookings_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.framemain.Navigate(new PageMyBookings());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.framemain.Navigate(new PageLogin());
        }
    }
}