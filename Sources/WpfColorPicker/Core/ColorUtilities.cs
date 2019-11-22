//////////////////////////////////////////////
// 2006 - Microsoft / Sample
//        https://blogs.msdn.microsoft.com/wpfsdk/2006/10/26/uncommon-dialogs-font-chooser-color-picker-dialogs/ 
//
// 2019 - Forked by Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Windows.Media;
using System.Collections.Generic;

namespace WpfColorPicker.Core
{
    static class ColorUtilities
    {
        /// <summary>
        /// Converts an RGB color to an HSV color.
        /// </summary>
        public static HsvColor ConvertRgbToHsv(int r, int b, int g)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            s = v == 0.0 ? 0 : delta / v;

            if (s == 0)
                h = 0.0;
            else
            {
                if (r == v)
                    h = (g - b) / delta;
                else if (g == v)
                    h = 2 + (b - r) / delta;
                else if (b == v)
                    h = 4 + (r - g) / delta;

                h *= 60;
                if (h < 0.0)
                    h += 360;
            }

            HsvColor hsvColor = new HsvColor
            {
                H = h,
                S = s,
                V = v / 255
            };

            return hsvColor;
        }

        /// <summary>
        /// Converts an HSV color to an RGB color.
        /// </summary>
        public static Color ConvertHsvToRgb(double h, double s, double v)
        {
            double r, g, b;

            if (s == 0)
                r = g = b = v;
            else
            {
                int i;
                double f, p, q, t;

                h = h == 360 ? 0 : h / 60;

                i = (int)Math.Truncate(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;
                    default:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }

            return Color.FromArgb(255, (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        /// <summary>
        /// Generates a list of colors with hues ranging from 0 360 and a saturation and value of 1.
        /// </summary>
        public static List<Color> GenerateHsvSpectrum()
        {
            List<Color> colorsList = new List<Color>(8);

            for (int i = 0; i < 29; i++)
                colorsList.Add
                (
                    ConvertHsvToRgb(i * 12, 1, 1)
                );

            colorsList.Add(ConvertHsvToRgb(0, 1, 1));
            
            return colorsList;
        }
    }
}