using System;
using System.Diagnostics;
using System.Linq;

namespace NCuid
{
    public class Cuid
    {
        private static ulong _globalCounter;
        private const int BlockSize                  = 4;
        private const int Base                       = 36;
        private static readonly ulong DiscreteValues = (ulong)Math.Pow(Base, BlockSize);

        private static string RandomBlock(Random rnd)
        {
            var number = (long)(rnd.NextDouble() * DiscreteValues);
            number <<= 0;

            var r = number.ToBase36().Pad(BlockSize);

            return r;
        }

        public static string Generate()
        {
            var ts          = DateTime.Now.ToUnixMilliTime().ToBase36();
            var gen         = new Random();
            var rnd         = RandomBlock(gen) + RandomBlock(gen);
            var fingerprint = FingerPrint();

            _globalCounter = (_globalCounter < DiscreteValues) 
                ? _globalCounter 
                : 0;

            var counter = _globalCounter.ToBase36().Pad(BlockSize);

            _globalCounter++;

            return ("c" + ts + counter + fingerprint + rnd).ToLowerInvariant();
        }

        public static string FingerPrint()
        {
            const int padding = 2;

            var pid         = Base36Converter.ToBase36((Process.GetCurrentProcess().Id)).Pad(padding);
            var hostname    = Environment.MachineName;
            var length      = hostname.Length;
            var inputNumber = hostname.Split().Aggregate(length + 36, (prev, c) => prev + c[0]);

            var hostId = Base36Converter.ToBase36(inputNumber).Pad(padding);
            return pid + hostId;
        }
    }
}
