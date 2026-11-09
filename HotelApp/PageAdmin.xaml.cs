using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelApp.Data;

namespace HotelApp
{
    public partial class PageAdmin : Page
    {
        private Rooms selectedRoom;
        private string selectedImageName = "";

        public PageAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            cbRoomType.ItemsSource = AppConnect.model0db.RoomCategories.ToList();
        }

        private void LoadData()
        {
            dgRooms.ItemsSource = AppConnect.model0db.Rooms.ToList();
        }

        private void dgRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRoom = dgRooms.SelectedItem as Rooms;
            if (selectedRoom != null)
            {
                txtRoomNumber.Text = selectedRoom.Number;
                txtPrice.Text = selectedRoom.PricePerNight.ToString();
                txtCapacity.Text = selectedRoom.Capacity.ToString();
                txtDesc.Text = selectedRoom.Description;
                cbRoomType.SelectedValue = selectedRoom.CategoryId;

                selectedImageName = selectedRoom.ImagePath;
                txtImagePath.Text = selectedImageName;
            }
        }

        // ⭐ ВЫБОР КАРТИНКИ (сохраняем ПОЛНЫЙ путь к файлу)
        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Images|*.png;*.jpg;*.jpeg"
                };

                if (ofd.ShowDialog() == true)
                {
                    // Берем полный путь (например, C:\Users\...\Desktop\room4.jpg)
                    selectedImageName = ofd.FileName;
                    txtImagePath.Text = selectedImageName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора файла: {ex.Message}");
            }
        }

        // ⭐ ДОБАВЛЕНИЕ НОМЕРА
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) || cbRoomType.SelectedValue == null)
                {
                    MessageBox.Show("Заполните номер и выберите категорию!");
                    return;
                }

                var room = new Rooms
                {
                    Number = txtRoomNumber.Text,
                    PricePerNight = decimal.Parse(txtPrice.Text),
                    Capacity = int.Parse(txtCapacity.Text),
                    Description = txtDesc.Text,
                    CategoryId = (int)cbRoomType.SelectedValue,
                    ImagePath = selectedImageName // Сохраняем полный путь
                };

                AppConnect.model0db.Rooms.Add(room);
                AppConnect.model0db.SaveChanges();

                LoadData();
                MessageBox.Show("Номер успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // ⭐ РЕДАКТИРОВАНИЕ НОМЕРА
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoom == null) { MessageBox.Show("Выберите номер из таблицы!"); return; }

            try
            {
                // Находим запись в БД по ID, чтобы EF понял, что мы её обновляем
                var roomToUpdate = AppConnect.model0db.Rooms.Find(selectedRoom.Id);

                if (roomToUpdate != null)
                {
                    roomToUpdate.Number = txtRoomNumber.Text;
                    roomToUpdate.PricePerNight = decimal.Parse(txtPrice.Text);
                    roomToUpdate.Capacity = int.Parse(txtCapacity.Text);
                    roomToUpdate.Description = txtDesc.Text;
                    roomToUpdate.CategoryId = (int)cbRoomType.SelectedValue;

                    // Обновляем картинку (если выбрали новую)
                    if (!string.IsNullOrEmpty(selectedImageName))
                    {
                        roomToUpdate.ImagePath = selectedImageName;
                    }

                    AppConnect.model0db.SaveChanges();
                    LoadData();
                    MessageBox.Show("Номер успешно обновлен!");
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null) msg += "\n\n" + ex.InnerException.Message;
                MessageBox.Show(msg, "Ошибка БД");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoom == null) return;
            try
            {
                AppConnect.model0db.Rooms.Remove(selectedRoom);
                AppConnect.model0db.SaveChanges();
                LoadData();
                MessageBox.Show("Номер удален!");
            }
            catch
            {
                MessageBox.Show("Невозможно удалить номер, так как на него есть активные бронирования!", "Ошибка");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) { AppFrame.framemain.Navigate(new PageLogin()); }
    }
}