using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfColorPicker.CustomControls
{
    public class ComboBoxColorPicker : ComboBox
    {
        //static ComboBoxColorPicker() =>
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxColorPicker),
        //        new FrameworkPropertyMetadata(typeof(ComboBoxColorPicker)));

        public ComboBoxColorPicker() =>
            ItemsSource = typeof(System.Windows.Media.Colors).GetProperties();
    }
}
