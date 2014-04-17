using Xunit;

namespace Cuid.Tests
{
    public class Base36ConverterTests
    {
        [Fact]
        public void DecodeTest()
        {
            Assert.Equal(0, Base36Converter.Decode(""));
            Assert.Equal(-1, Base36Converter.Decode("%"));
            Assert.Equal(1412823931503067241, Base36Converter.Decode("AQF8AA0006EH"));
        }

        [Fact]
        public void EncodeTest()
        {
            Assert.Equal("AQF8AA0006EH", Base36Converter.Encode(1412823931503067241));
        }
    }
}
