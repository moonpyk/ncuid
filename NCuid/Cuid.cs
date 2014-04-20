using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;

namespace NCuid
{
    /// <summary>
    /// Utility class to generate CUIDs see https://github.com/dilvie/cuid for complete manifesto
    /// </summary>
    public static class Cuid
    {
        private const int BlockSize                  = 4;
        private const int Base                       = 36;
        private static readonly ulong DiscreteValues = (ulong)Math.Pow(Base, BlockSize);

        private static ulong _globalCounter;
        private static string _hostname;

        private static ulong SafeCounter
        {
            get
            {
                _globalCounter = (_globalCounter < DiscreteValues) 
                    ? _globalCounter 
                    : 0;

                _globalCounter++;

                return _globalCounter - 1;
            }
        }

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
                catch (SecurityException) // Screw it
                {
                    _hostname = new Random().Next().ToString(CultureInfo.InvariantCulture);
                }

                return _hostname;
            }
        }

        /// <summary>
        /// Returns a short sequential random string with some collision-busting measures
        /// </summary>
        /// <returns>A 25 characters string</returns>
        public static string Generate()
        {
            var ts          = DateTime.Now.ToUnixMilliTime().ToBase36();
            var gen         = new Random();
            var rnd         = RandomBlock(gen) + RandomBlock(gen);
            var fingerprint = FingerPrint();

            var counter = SafeCounter.ToBase36().Pad(BlockSize);

            return ("c" + ts + counter + fingerprint + rnd).ToLowerInvariant();
        }

        /// <summary>
        /// Return a short (slugged) version of a CUID, likely to be less sequencial
        /// </summary>
        /// <returns>A 7 to 10 characters string (depending of the internal counter value)</returns>
        public static string Slug()
        {
            var print   = FingerPrint().Slice(0, 1) + FingerPrint().Slice(-1);
            var rnd     = RandomBlock(new Random()).Slice(-2);
            var counter = SafeCounter.ToBase36().Slice(-4);
            var dt      = DateTime.Now.ToUnixMilliTime().ToBase36();

            return (dt.Slice(-2) + counter + print + rnd).ToLowerInvariant();
        }

        /// <summary>
        /// Generates a host fingerprint, using when possible the machine name and the current process pid.
        /// If access to the machine name is refused by the framework, a random number based machine name is generated once,
        /// and kept for further calls.
        /// </summary>
        /// <returns>A 4 character string</returns>
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
