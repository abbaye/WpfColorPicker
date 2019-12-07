//////////////////////////////////////////////
// 2006 - Microsoft / Sample
//        https://blogs.msdn.microsoft.com/wpfsdk/2006/10/26/uncommon-dialogs-font-chooser-color-picker-dialogs/ 
//
// 2019 - Forked by Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using WpfColorPicker.Core;

namespace WpfColorPicker.CustomControls
{
    /// <summary>
    /// An HSB (hue, saturation, brightness) based color picker.
    /// </summary>
    public class ColorPicker : Control
    {
        #region Global variables
        private SpectrumSlider _ColorSlider;
        private FrameworkElement _ColorDetail;
        private Path _ColorMarker;
        private const string _ColorMarkerName = "PART_ColorMarker";
        private const string _ColorSliderName = "PART_ColorSlider";
        private const string _ColorDetailName = "PART_ColorDetail";
        private readonly TranslateTransform _markerTransform = new TranslateTransform();
        private Point? _ColorPosition;
        private Color _color;
        private bool _shouldFindPoint;
        private bool _templateApplied;
        private bool _isAlphaChange;
        #endregion

        static ColorPicker() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), 
                new FrameworkPropertyMetadata(typeof(ColorPicker)));

        public ColorPicker()
        {
            _templateApplied = false;
            _color = Colors.White;
            _shouldFindPoint = true;

            SetValue(AProperty, _color.A);
            SetValue(RProperty, _color.R);
            SetValue(GProperty, _color.G);
            SetValue(BProperty, _color.B);
            SetValue(SelectedColorProperty, _color);
        }

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ColorDetail = GetTemplateChild(_ColorDetailName) as FrameworkElement;
            _ColorMarker = GetTemplateChild(_ColorMarkerName) as Path;
            _ColorSlider = GetTemplateChild(_ColorSliderName) as SpectrumSlider;
            _ColorSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(BaseColorChanged);
            _ColorMarker.RenderTransform = _markerTransform;
            _ColorMarker.RenderTransformOrigin = new Point(0.5, 0.5);
            _ColorDetail.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
            _ColorDetail.PreviewMouseMove += new MouseEventHandler(OnMouseMove);
            _ColorDetail.SizeChanged += new SizeChangedEventHandler(ColorDetailSizeChanged);

            _templateApplied = true;
            _shouldFindPoint = true;
            _isAlphaChange = false;

            SelectedColor = _color;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set
            {
                SetValue(SelectedColorProperty, _color);
                SetColor((Color)value);
            }
        }

        #region RGB Properties
        /// <summary>
        /// Gets or sets the ARGB alpha value of the selected color.
        /// </summary>
        public byte A
        {
            get => (byte)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }

        /// <summary>
        /// Gets or sets the ARGB red value of the selected color.
        /// </summary>
        public byte R
        {
            get => (byte)GetValue(RProperty);
            set => SetValue(RProperty, value);
        }

        /// <summary>
        /// Gets or sets the ARGB green value of the selected color.
        /// </summary>
        public byte G
        {
            get => (byte)GetValue(GProperty);
            set => SetValue(GProperty, value);
        }

        /// <summary>
        /// Gets or sets the ARGB blue value of the selected color.
        /// </summary>
        public byte B
        {
            get => (byte)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }
        #endregion RGB Properties

        #region ScRGB Properties

        /// <summary>
        /// Gets or sets the ScRGB alpha value of the selected color.
        /// </summary>
        public double ScA
        {
            get => (double)GetValue(ScAProperty);
            set => SetValue(ScAProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB red value of the selected color.
        /// </summary>
        public double ScR
        {
            get => (double)GetValue(ScRProperty);
            set => SetValue(RProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB green value of the selected color.
        /// </summary>
        public double ScG
        {
            get => (double)GetValue(ScGProperty);
            set => SetValue(GProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB blue value of the selected color.
        /// </summary>
        public double ScB
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }
        #endregion ScRGB Properties

        /// <summary>
        /// Gets or sets the the selected color in hexadecimal notation.
        /// </summary>
        public string HexadecimalString
        {
            get => (string)GetValue(HexadecimalStringProperty);
            set => SetValue(HexadecimalStringProperty, value);
        }
        #endregion
        
        #region Public Events

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add
            {
                AddHandler(SelectedColorChangedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedColorChangedEvent, value);
            }
        }

        #endregion
        
        #region Dependency Property Fields
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register (nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(Colors.Transparent, new PropertyChangedCallback(SelectedColor_changed)));

        public static readonly DependencyProperty ScAProperty =
            DependencyProperty.Register(nameof(ScA), typeof(float), typeof(ColorPicker),
               new PropertyMetadata((float)1, new PropertyChangedCallback(ScAChanged)));

        public static readonly DependencyProperty ScRProperty =
            DependencyProperty.Register(nameof(ScR), typeof(float), typeof(ColorPicker),
                new PropertyMetadata((float)1, new PropertyChangedCallback(ScRChanged)));

        public static readonly DependencyProperty ScGProperty =
            DependencyProperty.Register(nameof(ScG), typeof(float), typeof(ColorPicker),
                new PropertyMetadata((float)1, new PropertyChangedCallback(ScGChanged)));

        public static readonly DependencyProperty ScBProperty =
            DependencyProperty.Register(nameof(ScB), typeof(float), typeof(ColorPicker),
                new PropertyMetadata((float)1, new PropertyChangedCallback(ScBChanged)));

        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(byte), typeof(ColorPicker),
                new PropertyMetadata((byte)255, new PropertyChangedCallback(AChanged)));

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(ColorPicker),
                new PropertyMetadata((byte)255, new PropertyChangedCallback(RChanged)));

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(ColorPicker),
                new PropertyMetadata((byte)255, new PropertyChangedCallback(GChanged)));

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(ColorPicker),
                new PropertyMetadata((byte)255, new PropertyChangedCallback(BChanged)));

        public static readonly DependencyProperty HexadecimalStringProperty =
            DependencyProperty.Register(nameof(HexadecimalString), typeof(string), typeof(ColorPicker),
                new PropertyMetadata("#FFFFFFFF", new PropertyChangedCallback(HexadecimalStringChanged)));
        #endregion

        #region RoutedEvent Fields

        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(SelectedColorChanged), RoutingStrategy.Bubble, 
                typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPicker));
        #endregion

        #region Property Changed Callbacks

        private static void AChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnAChanged((byte)e.NewValue);
        }

        protected virtual void OnAChanged(byte newValue)
        {
            _color.A = newValue;
            SetValue(ScAProperty, _color.ScA);
            SetValue(SelectedColorProperty, _color);
        }

        private static void RChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnRChanged((byte)e.NewValue);
        }

        protected virtual void OnRChanged(byte newValue)
        {
            _color.R = newValue;
            SetValue(ScRProperty, _color.ScR);
            SetValue(SelectedColorProperty, _color);
        }

        private static void GChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnGChanged((byte)e.NewValue);
        }

        protected virtual void OnGChanged(byte newValue)
        {
            _color.G = newValue;
            SetValue(ScGProperty, _color.ScG);
            SetValue(SelectedColorProperty, _color);
        }

        private static void BChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnBChanged((byte)e.NewValue);
        }

        protected virtual void OnBChanged(byte newValue)
        {
            _color.B = newValue;
            SetValue(ScBProperty, _color.ScB);
            SetValue(SelectedColorProperty, _color);
        }

        private static void ScAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScAChanged((float)e.NewValue);
        }

        protected virtual void OnScAChanged(float newValue)
        {
            _isAlphaChange = true;
            if (_shouldFindPoint)
            {
                _color.ScA = newValue;
                SetValue(AProperty, _color.A);
                SetValue(SelectedColorProperty, _color);
                SetValue(HexadecimalStringProperty, _color.ToString());
            }
            _isAlphaChange = false;
        }
        
        private static void ScRChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScRChanged((float)e.NewValue);
        }

        protected virtual void OnScRChanged(float newValue)
        {
            if (!_shouldFindPoint) return;
            
            _color.ScR = newValue;
            SetValue(RProperty, _color.R);
            SetValue(SelectedColorProperty, _color);
            SetValue(HexadecimalStringProperty, _color.ToString());
        }
        
        private static void ScGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScGChanged((float)e.NewValue);
        }

        protected virtual void OnScGChanged(float newValue)
        {
            if (!_shouldFindPoint) return;
            
            _color.ScG = newValue;
            SetValue(GProperty, _color.G);
            SetValue(SelectedColorProperty, _color);
            SetValue(HexadecimalStringProperty, _color.ToString());
        }

        private static void ScBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScBChanged((float)e.NewValue);
        }

        protected virtual void OnScBChanged(float newValue)
        {
            if (!_shouldFindPoint) return;
            
            _color.ScB = newValue;
            SetValue(BProperty, _color.B);
            SetValue(SelectedColorProperty, _color);
            SetValue(HexadecimalStringProperty, _color.ToString());
        }

        private static void HexadecimalStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnHexadecimalStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<En attente>")]
        protected virtual void OnHexadecimalStringChanged(string oldValue, string newValue)
        {
            try
            {
                if (_shouldFindPoint)
                    _color = (Color)ColorConverter.ConvertFromString(newValue);
                
                SetValue(AProperty, _color.A);
                SetValue(RProperty, _color.R);
                SetValue(GProperty, _color.G);
                SetValue(BProperty, _color.B);
                
                if (_shouldFindPoint && !_isAlphaChange && _templateApplied)
                    UpdateMarkerPosition(_color);
            }
            catch (FormatException)
            {
                SetValue(HexadecimalStringProperty, oldValue);
            }
        }

        private static void SelectedColor_changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cPicker = (ColorPicker)d;
            cPicker.OnSelectedColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        protected virtual void OnSelectedColorChanged(Color oldColor, Color newColor)
        {
            RoutedPropertyChangedEventArgs<Color> newEventArgs =
                new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor)
                {
                    RoutedEvent = SelectedColorChangedEvent
                };

            RaiseEvent(newEventArgs);
        }

        #endregion
        
        #region Template Part Event Handlers
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            _templateApplied = false;
            if (oldTemplate != null)
            {
                _ColorSlider.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(BaseColorChanged);
                _ColorDetail.MouseLeftButtonDown -= new MouseButtonEventHandler(OnMouseLeftButtonDown);
                _ColorDetail.PreviewMouseMove -= new MouseEventHandler(OnMouseMove);
                _ColorDetail.SizeChanged -= new SizeChangedEventHandler(ColorDetailSizeChanged);
                _ColorDetail = null;
                _ColorMarker = null;
                _ColorSlider = null;
            }
            base.OnTemplateChanged(oldTemplate, newTemplate);
            
        }
        
        private void BaseColorChanged(object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            if (_ColorPosition != null)
                DetermineColor((Point)_ColorPosition);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(_ColorDetail);
            UpdateMarkerPosition(p);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            Point p = e.GetPosition(_ColorDetail);
            UpdateMarkerPosition(p);
            Mouse.Synchronize();
        }

        private void ColorDetailSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (args.PreviousSize != Size.Empty &&
                args.PreviousSize.Width != 0 &&
                args.PreviousSize.Height != 0)
            {
                var widthDifference = args.NewSize.Width / args.PreviousSize.Width;
                var heightDifference = args.NewSize.Height / args.PreviousSize.Height;
                _markerTransform.X *= widthDifference;
                _markerTransform.Y *= heightDifference;
            }
            else if (_ColorPosition != null)
            {
                _markerTransform.X = ((Point)_ColorPosition).X * args.NewSize.Width;
                _markerTransform.Y = ((Point)_ColorPosition).Y * args.NewSize.Height;
            }
        }
        #endregion
        
        #region Color Resolution Helpers

        private void SetColor(Color theColor)
        {
            _color = theColor;

            if (!_templateApplied) return;
            
            SetValue(AProperty, _color.A);
            SetValue(RProperty, _color.R);
            SetValue(GProperty, _color.G);
            SetValue(BProperty, _color.B);
            UpdateMarkerPosition(theColor);
        }

        private void UpdateMarkerPosition(Point p)
        {
            _markerTransform.X = p.X;
            _markerTransform.Y = p.Y;
            p.X /= _ColorDetail.ActualWidth;
            p.Y /= _ColorDetail.ActualHeight;
            _ColorPosition = p;
            DetermineColor(p);
        }

        private void UpdateMarkerPosition(Color theColor)
        {
            _ColorPosition = null;
            
            HsvColor hsv = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B);

            _ColorSlider.Value = hsv.H;

            Point p = new Point(hsv.S, 1 - hsv.V);

            _ColorPosition = p;
            p.X *= _ColorDetail.ActualWidth;
            p.Y *= _ColorDetail.ActualHeight;
            _markerTransform.X = p.X;
            _markerTransform.Y = p.Y;
        }

        private void DetermineColor(Point p)
        {
            HsvColor hsv = new HsvColor(360 - _ColorSlider.Value, 1, 1)
            {
                S = p.X,
                V = 1 - p.Y
            };

            _color = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            _shouldFindPoint = false;
            _color.ScA = (float)GetValue(ScAProperty);
            SetValue(HexadecimalStringProperty, _color.ToString());
            _shouldFindPoint = true;
        }

        #endregion
    }
}