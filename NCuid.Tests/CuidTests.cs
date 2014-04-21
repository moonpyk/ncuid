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
            var cuid       = Cuid.Generate();
            var cuidSecure = Cuid.Generate(RandomSource.Secure);

            Debug.WriteLine(cuid);
            Debug.WriteLine(cuidSecure);

            Assert.Equal(25, cuid.Length);
            Assert.Equal(25, cuidSecure.Length);

            Assert.Throws<IndexOutOfRangeException>(() => Cuid.Generate((RandomSource) 3));
        }

        [Fact]
        public void SlugTest()
        {
            var slug       = Cuid.Slug();
            var slugSecure = Cuid.Slug(RandomSource.Secure);

            Debug.WriteLine(slug);
            Debug.WriteLine(slugSecure);

            Assert.InRange(slug.Length, 7, 10);
            Assert.InRange(slugSecure.Length, 7, 10);

            Assert.Throws<IndexOutOfRangeException>(() => Cuid.Slug((RandomSource)3));
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
    
            l.Clear();

            for (var i = 0; i <= 1200000; i++)
            {
                var gen = Cuid.Generate(RandomSource.Secure);
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

            l.Clear();

            for (var i = 0; i <= 1200000; i++)
            {
                var gen = Cuid.Slug(RandomSource.Secure);
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
    }
}
