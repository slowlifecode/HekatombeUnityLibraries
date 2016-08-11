using UnityEngine;
using System;
using System.Collections.Generic;

namespace Hekatombe.Base
{
    public static class ColorExtension
	{
        public static Color Transparent(this Color color)
        {
        	return color.Alpha (0);
        }

        public static Color Opaque(this Color color)
        {
        	return color.Alpha (1);
        }

        public static Color Alpha(this Color color, float alpha)
        {
        	color.a = alpha;
        	return color;
        }

        public static string ColorToHex(this Color32 color)
        {
            string hex = string.Format ("#{0:X2}{1:X2}{2:X2}", color);
            return hex;
        }

        public static Color32 HexToColor(this string hex)
        {
            try {
                hex = hex.Replace ("#", string.Empty);
                if (hex.Length != 6 && hex.Length != 8) 
					throw new Exception("Color doesn't have 6 or 8 digits: " + hex);
                byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
                byte a = 255;
                if (hex.Length>6) 
					a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
                return new Color32(r,g,b,a);
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                //pink Color
                return new Color32(255,0,255,255);
            }
        }
    }
}