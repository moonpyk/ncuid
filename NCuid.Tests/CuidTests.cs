using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace NCuid.Tests
{
    public class CuidTests
    {
        [Fact]
        public void GenerateTest()
        {
            var cuid = Cuid.Generate();

            Debug.WriteLine(cuid);

            Assert.Equal(25, cuid.Length);
        }

        [Fact]
        public void NoReasonableCollision()
        {
            var l = new HashSet<string>();

            for (var i = 0; i <= 1200000; i++)
            {
                var gen = Cuid.Generate();
                Assert.False(l.Contains(gen));

                Assert.True(l.Add(gen));
            }            
        }

        [Fact]
        public void NoSlugReasonableCollisionTest()
        {
            var l = new HashSet<string>();

            for (var i = 0; i <= 1200000; i++)
            {
                var gen = Cuid.Slug();
                Assert.False(l.Contains(gen));

                Assert.True(l.Add(gen));
            }
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

            Assert.InRange(slug.Length, 7, 10);
        }
    }
}
