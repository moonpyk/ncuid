using System;
using System.Diagnostics;
using System.Linq;

namespace Cuid
{
    public class Cuid
    {
        private static ulong _globalCounter;
        private const int BlockSize                  = 4;
        private const int Base                       = 36;
        private static readonly ulong DiscreteValues = (ulong)Math.Pow(Base, BlockSize);

        //private static string Pad(string num, int size)
        //{
        //    var s = "0000 0 0000" + num;
        //    return s.Substring(s.Length - size);
        //}

        private static string Pad(string num, int size)
        {
            var s = "000000000" + num;
            return s.Substring(s.Length-size);
        }

        private static string RandomBlock(Random rnd)
        {
            var number = (long)(rnd.NextDouble() * DiscreteValues);
            number <<= 0;

            var r = Pad(Base36Converter.Encode(number),
                BlockSize
            );

            return r;
        }

        public static string Generate()
        {
            var ts          = Base36Converter.Encode(DateTime.Now.ToUnixMilliTime());
            var gen         = new Random();
            var rnd         = RandomBlock(gen) + RandomBlock(gen);
            var fingerprint = FingerPrint();

            _globalCounter = (_globalCounter < DiscreteValues) 
                ? _globalCounter 
                : 0;

            var counter = Pad(Base36Converter.Encode(_globalCounter), BlockSize);

            _globalCounter++;

            return ("c" + ts + counter + fingerprint + rnd).ToLowerInvariant();
        }

        public static string FingerPrint()
        {
            const int padding = 2;

            var pid         = Pad(Base36Converter.Encode((Process.GetCurrentProcess().Id)), padding);
            var hostname    = Environment.MachineName;
            var length      = hostname.Length;
            var inputNumber = hostname.Split().Aggregate(length + 36, (prev, c) => prev + c[0]);

            var hostId = Pad(Base36Converter.Encode(inputNumber), padding);
            return pid + hostId;
        }
    }
}
