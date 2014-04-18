using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;

namespace NCuid
{
    public static class Cuid
    {
        private const int BlockSize                  = 4;
        private const int Base                       = 36;
        private static readonly ulong DiscreteValues = (ulong)Math.Pow(Base, BlockSize);

        private static ulong _globalCounter;
        private static string _hostname;

        private static string Hostname
        {
            get
            {
                if (_hostname != null)
                {
                    return _hostname;
                }

                try
                {
                    _hostname = Environment.MachineName;
                }
                catch (SecurityException) // Fuck it
                {
                    _hostname = new Random().Next().ToString(CultureInfo.InvariantCulture);
                }

                return _hostname;
            }
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

        public static string Slug()
        {
            var dt      = DateTime.Now.ToUnixMilliTime().ToBase36();
            var counter = _globalCounter.ToBase36().Slice(-1);
            var print   = FingerPrint().Slice(0, 1) + FingerPrint().Slice(-1);
            var rnd     = RandomBlock(new Random()).Slice(-1);

            _globalCounter++;

            return (dt.Slice(2, 4) + dt.Slice(-2) + counter + print + rnd).ToLowerInvariant();
        }

        public static string FingerPrint()
        {
            const int padding = 2;

            var pid         = Base36Converter.ToBase36((Process.GetCurrentProcess().Id)).Pad(padding);
            var hostname    = Hostname;
            var length      = hostname.Length;
            var inputNumber = hostname.Split().Aggregate(length + 36, (prev, c) => prev + c[0]);

            var hostId = Base36Converter.ToBase36(inputNumber).Pad(padding);
            return pid + hostId;
        }

        private static string RandomBlock(Random rnd)
        {
            var number = (long)(rnd.NextDouble() * DiscreteValues);

            var r = number.ToBase36().Pad(BlockSize);

            return r;
        }
    }
}
