using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class BookingWindow : Window
    {
        private Rooms _currentRoom;
        private decimal _totalPrice = 0;

        public BookingWindow(Rooms room)
        {
            InitializeComponent();
            _currentRoom = room;
            tbTitle.Text = $"Номер {room.Number}";

            // ⭐ 1. ОГРАНИЧЕНИЕ КАЛЕНДАРЕЙ (10 лет)
            dpCheckIn.DisplayDateStart = DateTime.Today;
            dpCheckIn.DisplayDateEnd = DateTime.Today.AddYears(10);

            dpCheckOut.DisplayDateStart = DateTime.Today;
            dpCheckOut.DisplayDateEnd = DateTime.Today.AddYears(10);

            // ⭐ 2. АВТОПОДСТАНОВКА ИМЕНИ
            txtGuestName.Text = AppConnect.UserName;
        }

        private void Date_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dpCheckIn.SelectedDate.HasValue && dpCheckOut.SelectedDate.HasValue)
            {
                DateTime start = dpCheckIn.SelectedDate.Value;
                DateTime end = dpCheckOut.SelectedDate.Value;

                if (end <= start) { tbTotalPrice.Text = "0.00 ₽"; return; }

                int days = (end - start).Days;
                _totalPrice = days * _currentRoom.PricePerNight;
                tbTotalPrice.Text = $"{_totalPrice:F2} ₽ (за {days} ноч.)";
            }
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpCheckIn.SelectedDate.HasValue || !dpCheckOut.SelectedDate.HasValue)
                {
                    MessageBox.Show("Выберите даты!");
                    return;
                }

                if (dpCheckOut.SelectedDate.Value <= dpCheckIn.SelectedDate.Value)
                {
                    MessageBox.Show("Дата выезда должна быть позже!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtGuestName.Text))
                {
                    MessageBox.Show("Введите имя гостя!");
                    return;
                }

                // Создаем бронь
                var booking = new Bookings
                {
                    UserId = AppConnect.CurrentUserId,
                    RoomId = _currentRoom.Id,
                    StatusId = 1, // 1 = Новая
                    CheckInDate = dpCheckIn.SelectedDate.Value,
                    CheckOutDate = dpCheckOut.SelectedDate.Value,
                    TotalPrice = _totalPrice
                };

                AppConnect.model0db.Bookings.Add(booking);
                AppConnect.model0db.SaveChanges();

                // Создаем гостя и привязываем к брони
                var guest = new Guests
                {
                    BookingId = booking.Id, // Привязка
                    FirstName = txtGuestName.Text,
                    LastName = "",
                    PassportData = txtPassport.Text
                };

                AppConnect.model0db.Guests.Add(guest);
                AppConnect.model0db.SaveChanges();

                MessageBox.Show($"Номер успешно забронирован!\nСумма: {_totalPrice:F2} ₽");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}