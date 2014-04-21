using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

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
        /// Returns a short sequential random string with some collision-busting measures (a CUID)
        /// </summary>
        /// <param name="rs">Type of the random source to use, if not specified <see cref="RandomSource.Simple"/> is used.</param>
        /// <returns>A 25 characters string</returns>
        public static string Generate(RandomSource rs = RandomSource.Simple)
        {
            var ts          = DateTime.Now.ToUnixMilliTime().ToBase36();
            var fingerprint = FingerPrint();

            string rnd;

            switch (rs)
            {
                case RandomSource.Secure:
                    {
                        using (var gen = new RNGCryptoServiceProvider())
                        {
                            rnd = SecureRandomBlock(gen) + SecureRandomBlock(gen);
                        }
                    }
                    break;
                case RandomSource.Simple:
                    {
                        var gen = new Random();
                        rnd = SimpleRandomBlock(gen) + SimpleRandomBlock(gen);
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException("Invalid RandomSource specified");
            }

            var counter = SafeCounter().ToBase36().Pad(BlockSize);

            return ("c" + ts + counter + fingerprint + rnd).ToLowerInvariant();
        }

        /// <summary>
        /// Return a short (slugged) version of a CUID, likely to be less sequential
        /// </summary>
        /// <param name="rs">Type of the random source to use, if not specified <see cref="RandomSource.Simple"/> is used.</param>
        /// <returns>A 7 to 10 characters string (depending of the internal counter value)</returns>
        public static string Slug(RandomSource rs = RandomSource.Simple)
        {
            var print   = FingerPrint().Slice(0, 1) + FingerPrint().Slice(-1);
            var counter = SafeCounter().ToBase36().Slice(-4);
            var dt      = DateTime.Now.ToUnixMilliTime().ToBase36();

            string rnd;

            switch (rs)
            {
                case RandomSource.Secure:
                    using (var gen = new RNGCryptoServiceProvider())
                    {
                        rnd = SecureRandomBlock(gen).Slice(-2);
                    }
                    break;
                case RandomSource.Simple:
                    rnd = SimpleRandomBlock(new Random()).Slice(-2);
                    break;

                default:
                    throw new IndexOutOfRangeException("Invalid RandomSource specified");
            }

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

        private static ulong SafeCounter()
        {
            _globalCounter = (_globalCounter < DiscreteValues)
                ? _globalCounter
                : 0;
            
            _globalCounter++;

            return _globalCounter - 1;
        }

        private static string SimpleRandomBlock(Random rnd)
        {
            var number = (long)(rnd.NextDouble() * DiscreteValues);

            var r = number.ToBase36().Pad(BlockSize);

            return r;
        }

        private static string SecureRandomBlock(RandomNumberGenerator gen)
        {
            var data = new byte[8];
            gen.GetNonZeroBytes(data);

            var baseNum =  ((double)BitConverter.ToUInt64(data, 0) / ulong.MaxValue);
            var number = (long)(baseNum * DiscreteValues);

            return number.ToBase36().Pad(BlockSize);
        }
    }
}
