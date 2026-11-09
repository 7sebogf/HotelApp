using System.Windows;
using HotelApp.Data;

namespace HotelApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Сохраняем фрейм для навигации
            AppFrame.framemain = MainFrame;
            // Запускаем страницу входа
            MainFrame.Navigate(new PageLogin());
        }
    }
}