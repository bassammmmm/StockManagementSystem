using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace StockManagementSystem.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool isLowStock
                ? MapBoolToColor(isLowStock)
                : Colors.Black;

        private static Color MapBoolToColor(bool isLowStock) =>
            isLowStock ? Colors.Red : Colors.Blue;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
