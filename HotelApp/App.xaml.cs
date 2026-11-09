using System;
using System.Windows;

namespace HotelApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Если программа упадет, вылезет понятное сообщение, а не креш
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = args.ExceptionObject as Exception;
                MessageBox.Show($"Критическая ошибка:\n{ex?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}