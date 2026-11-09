using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelApp.Data
{
    // Конвертер для выделения дорогих номеров (цена > 5000)
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price && price > 5000)
            {
                return "#FFF9C4"; // Светло-желтый
            }
            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}