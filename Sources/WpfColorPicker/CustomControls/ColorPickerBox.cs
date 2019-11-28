using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfColorPicker.CustomControls
{
    public class ColorPickerBox : ComboBox
    {
        static ColorPickerBox() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPickerBox),
                new FrameworkPropertyMetadata(typeof(ColorPickerBox)));

        public ColorPickerBox()
        {
            ItemsSource = GetAllColors();

            DataContext = this;
        }

        private IEnumerable<KeyValuePair<string, Color>> GetAllColors() => 
            typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<string, Color>(prop.Name, (Color)prop.GetValue(null)));
    }
}
