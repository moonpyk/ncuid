using System;

namespace NCuid
{
    internal static class StringExtensions
    {
        public static string Slice(this string source, int end)
        {
            return Slice(source, end, source.Length);
        }

        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end += source.Length;
            }

            if (start < 0)
            {
                start += source.Length;
            }

            if (start < 0)
            {
                start = 0;
            }

            return source.Substring(start, end - start); // Return Substring of length
        }

        public static string Reverse(this string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string Pad(this string s, int size)
        {
            var padded = s.PadLeft(size, '0');
            return padded.Substring(padded.Length - size);
        }
    }
}