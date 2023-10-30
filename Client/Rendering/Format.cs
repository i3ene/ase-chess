using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ase_chess.Client.Rendering
{
    public static class Format
    {
        public enum Color
        {
            BLACK = 0,
            RED = 1,
            GREEN = 2,
            BLUE = 6,
            GRAY = 7,
            BROWN = 52
        }

        /// <summary>
        /// Regular Expression for color code formatting
        /// </summary>
        public static readonly Regex col = new Regex("(\x1B).*?(m)");

        /// <summary>
        /// Clears all formatting on the string
        /// </summary>
        /// <param name="str">The string to clear the formatting from</param>
        /// <returns>Unformatted string</returns>
        public static string Clear(this string str)
        {
            return col.Replace(str, "");
        }

        /// <summary>
        /// Adds a format reset to the end of the string
        /// </summary>
        /// <param name="str">The string to add a format stop to</param>
        /// <returns>Formatted string</returns>
        public static string Reset(this string str)
        {
            return str + Col();
        }

        /// <summary>
        /// Convert length of a string with formatting
        /// </summary>
        /// <param name="str">String with formatting to convert length from</param>
        /// <param name="startIndex">Index to start from</param>
        /// <param name="length">Length to convert</param>
        /// <returns>Converted length wih formatting offset included</returns>
        public static int ToFormatLength(this string str, int startIndex, int length)
        {
            return str.FormatSubstring(startIndex, false).ToFormatIndex(length);
        }

        /// <summary>
        /// Converts a character index to a format index
        /// </summary>
        /// <param name="str">String to convert index to</param>
        /// <param name="index">Index to convert</param>
        /// <returns>Converted index</returns>
        public static int ToFormatIndex(this string str, int index)
        {
            foreach (Match match in Format.col.Matches(str))
            {
                if (match.Index > index) break;
                index += match.Length;
            }

            return index;
        }

        /// <summary>
        /// Converts a format index to a character index
        /// </summary>
        /// <param name="str">String to convert index from</param>
        /// <param name="index">Index to convert</param>
        /// <returns>Converted index</returns>
        public static int GetFormatIndex(this string str, int index)
        {
            foreach (Match match in Format.col.Matches(str))
            {
                if (match.Index > index) break;
                index -= match.Length;
            }

            return index;
        }

        /// <summary>
        /// Retrieve last used color of the string
        /// </summary>
        /// <param name="str">String with formatting</param>
        /// <param name="index">(Unformatted) index to get color from</param>
        /// <returns>Format of last used color</returns>
        public static string GetCol(this string str, int index)
        {
            string format = "";
            index = str.ToFormatIndex(index);
            foreach (Match match in Format.col.Matches(str))
            {
                if (match.Index > index) break;
                format = match.Value;
            }
            return format;
        }

        /// <summary>
        /// Retrieve length of string without formatting
        /// </summary>
        /// <param name="str">String with formatting to get the length from</param>
        /// <returns>Length of string without formatting</returns>
        public static int FormatLength(this string str)
        {
            int length = str.Length;
            foreach (Match match in Format.col.Matches(str))
            {
                length -= match.Length;
            }
            return length;
        }

        public static string Col(this string str, Color foreground, Color background)
        {
            return str.Col((int)foreground, (int)background);
        }

        public static string Col(this string str, int foreground, int background)
        {
            return str.Col(foreground).Col(background, true);
        }

        public static string Col(this string str, Color color, bool background = false)
        {
            return str.Col((int)color, background);
        }

        public static string Col(this string str, int color, bool background = false)
        {
            return $"{Col(color, background)}{str}".Reset();
        }

        /// <summary>
        /// Custom color
        /// </summary>
        /// <param name="color">(8-Bit ANSI) Color index</param>
        /// <param name="background">Is background color</param>
        /// <returns>Color format</returns>
        public static string Col(int color, bool background = false)
        {
            return $"\x1B[{(background ? 4 : 3)}8;5;{color}m";
        }

        /// <summary>
        /// Reset color
        /// </summary>
        /// <returns>Color format</returns>
        public static string Col()
        {
            return "\x1B[0m";
        }

        /// <summary>
        /// Retrieve substring taking formatting into account
        /// </summary>
        /// <param name="str">String with formatting</param>
        /// <param name="startIndex">(Unformatted) starting index</param>
        /// <param name="keepColor">To keep previous color</param>
        /// <returns>Substring with formatting</returns>
        public static string FormatSubstring(this string str, int startIndex, bool keepColor = true)
        {
            string format = keepColor ? str.GetCol(startIndex) : "";
            return format + str.Substring(str.ToFormatIndex(startIndex));
        }

        /// <summary>
        /// Retrieve substring taking formatting into account
        /// </summary>
        /// <param name="str">String with formatting</param>
        /// <param name="startIndex">(Unformatted) starting index</param>
        /// <param name="length">(Unformatted) length</param>
        /// <param name="keepColor">To keep previous color</param>
        /// <returns>Substring with formatting</returns>
        public static string FormatSubstring(this string str, int startIndex, int length, bool keepColor = true)
        {
            string format = keepColor ? str.GetCol(startIndex) : "";

            return format + str.Substring(str.ToFormatIndex(startIndex), str.ToFormatLength(startIndex, length));
        }

        public static string FormatPadRight(this string str, int totalWidth)
        {
            return str.PadRight(str.ToFormatIndex(totalWidth));
        }

        public static string FormatPadRight(this string str, int totalWidth, char paddingChar)
        {
            return str.PadRight(str.ToFormatIndex(totalWidth), paddingChar);
        }

        public static string FormatPadLeft(this string str, int totalWidth)
        {
            return str.PadLeft(str.ToFormatIndex(totalWidth));
        }

        public static string FormatPadLeft(this string str, int totalWidth, char paddingChar)
        {
            return str.PadLeft(str.ToFormatIndex(totalWidth), paddingChar);
        }
    }
}
