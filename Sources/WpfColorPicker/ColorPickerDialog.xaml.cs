//////////////////////////////////////////////
// 2006 - Microsoft 
//        https://blogs.msdn.microsoft.com/wpfsdk/2006/10/26/uncommon-dialogs-font-chooser-color-picker-dialogs/ 
//
// 2019 - Modified and adapted by Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System.Windows;
using System.Windows.Media;

namespace WpfColorPicker.CustomControls
{
    /// <summary>
    /// ColorPickerDialog used to pick colors
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        private Color _color = new Color();
        private Color _startingColor = new Color();

        public ColorPickerDialog() => InitializeComponent();

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            _color = cPicker.SelectedColor;
            DialogResult = true;
            Hide();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            DialogResult = false;
        }

        private void OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (e.NewValue == _color) return;

            OKButton.IsEnabled = true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            OKButton.IsEnabled = false;
            base.OnClosing(e);
        }

        public Color SelectedColor => _color;

        public Color StartingColor
        {
            get => _startingColor;
            set
            {
                cPicker.SelectedColor = value;
                OKButton.IsEnabled = false;
            }
        }
    }
}