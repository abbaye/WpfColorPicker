//////////////////////////////////////////////
// 2006 - Microsoft / Sample
//        https://blogs.msdn.microsoft.com/wpfsdk/2006/10/26/uncommon-dialogs-font-chooser-color-picker-dialogs/ 
//
// 2019 - Forked by Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

namespace WpfColorPicker
{
    /// <summary>
    /// Describes a color in terms of Hue, Saturation, and Value (brightness)
    /// </summary>
    public struct HsvColor
    {
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }

        public HsvColor(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }

        public override bool Equals(object obj) => throw new System.NotImplementedException();

        public override int GetHashCode() => throw new System.NotImplementedException();

        public static bool operator ==(HsvColor left, HsvColor right) => left.Equals(right);

        public static bool operator !=(HsvColor left, HsvColor right) => !(left == right);
    }
}