﻿using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace StockManagementSystem.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isLowStock)
            {
                return isLowStock ? Colors.Red : Colors.Blue;
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
