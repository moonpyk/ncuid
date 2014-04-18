using System;

namespace NCuid
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get the string slice between the two indexes.
        /// Inclusive for start index, exclusive for end index.
        /// </summary>
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end += source.Length;
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
            return padded.Substring(padded.Length-size);
        }
    }
}