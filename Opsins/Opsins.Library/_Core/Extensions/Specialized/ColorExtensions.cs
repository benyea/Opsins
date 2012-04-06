using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Opsins.Specialized
{
    /// <summary>
    /// Color扩展
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// 转换为RGB整数代码
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ToArgb(this Color color)
        {
            int argb = color.A << 24;
            argb += color.R << 16;
            argb += color.G << 8;
            argb += color.B;
            return argb;
        }

        /// <summary>
        /// 转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(this string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[1].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);
            }
        }
    }
}
