using Xunit;

namespace NCuid.Tests
{
    public class Base36ConverterTests
    {
        [Fact]
        public void EncodeTest()
        {
            Assert.Equal("0", Base36Converter.Encode(0));
            Assert.Equal("6TY", Base36Converter.Encode(8854));

            Assert.Equal("AQF8AA0006EH", 1412823931503067241.ToBase36());
        }

        [Fact]
        public void DecodeTest()
        {
            Assert.Equal(0, Base36Converter.Decode(""));
            Assert.Equal(0, Base36Converter.Decode("0"));
            Assert.Equal(-1, Base36Converter.Decode("%"));
            Assert.Equal(1412823931503067241, Base36Converter.Decode("AQF8AA0006EH"));
        }
    }
}
