using System;
using Xunit;

namespace NCuid.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void FromUnixTimeTest()
        {
            var dt = new DateTime(1987, 07, 03, 18, 00, 00);
            Assert.Equal(dt.ToUnixTime(), 552333600);
        }

        [Fact]
        public void ToUnixTimeTest()
        {
            var dt = new DateTime(1987, 07, 03, 18, 00, 00);

            Assert.Equal(dt, 552333600.FromUnixTime());
            Assert.Equal(dt, 552333600L.FromUnixTime());
        }
    }
}
