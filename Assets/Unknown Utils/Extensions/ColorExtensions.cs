using UnityEngine;

namespace Unknown.Extensions {

    public static class ColorExtensions {

        /// <summary>
        /// returns the color with alpha specified
        /// </summary>
        public static Color SetAlpha(this Color color, float alpha) {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// converts string hex code to color, returns white if parse failed
        /// </summary>
        public static Color HexToColor(this string hex) {
            if(ColorUtility.TryParseHtmlString(hex, out Color color)){
                return color;
            }

            return Color.white;
        }

        /// <summary>
        /// returns the hex code for the color 
        /// </summary>
        public static string ToHex(this Color color) {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }


        /// <summary>
        /// returns the inverted color of color provided
        /// </summary>
        public static Color Invert(this Color color) {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }

    }
}