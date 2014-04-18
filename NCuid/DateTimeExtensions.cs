using System;

namespace NCuid
{
    internal static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this int unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date - Epoch).TotalSeconds);
        }

        public static long ToUnixMilliTime(this DateTime date)
        {
            return Convert.ToInt64((date - Epoch).TotalMilliseconds);
        }
    }
}
