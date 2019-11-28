﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfColorPicker.Converter
{
    public class ColorToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            new SolidColorBrush((Color)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
