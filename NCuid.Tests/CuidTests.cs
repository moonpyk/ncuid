using System;
using System.Diagnostics;
using Xunit;
using NCuid;

namespace NCuid.Tests
{
    public class CuidTests
    {
        [Fact]
        public void GenerateTest()
        {
            var cuid = Cuid.Generate();

            Debug.WriteLine(cuid);

            Assert.Equal(cuid.Length, 25);
        }

        [Fact]
        public void CuidsAreShorterThanGuids()
        {
            Assert.True(
                Guid.NewGuid().ToString().Replace("-", string.Empty).Length > Cuid.Generate().Length
            );
        }

        [Fact]
        public void CuidsAreSlowerThanNativeGuids()
        {
            var toGen = Math.Pow(36, 4) +1;

            var sw = new Stopwatch();

            sw.Start();
            for (double i = 0; i < toGen; i++)
            {
                Cuid.Generate();
            }
            sw.Stop();

            var elapsedCuid = sw.ElapsedTicks;

            sw.Reset();

            sw.Start();
            for (double i = 0; i < toGen; i++)
            {
                Guid.NewGuid();
            }
            sw.Stop();

            var elapsedGuid = sw.ElapsedTicks;

            Assert.False(elapsedGuid > elapsedCuid);
        }

        [Fact]
        public void SlugTest()
        {
            var slug = Cuid.Slug();

            Debug.WriteLine(slug);

            Assert.Equal(slug.Length, 8);
        }
    }
}
