using System;
using System.Collections.Generic;

namespace NCuid
{
    internal static class Base36Converter
    {
        private const string Clist = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly char[] Clistarr = Clist.ToCharArray();

        public static string Encode(ulong inputNumber)
        {
            if (inputNumber == 0)
            {
                return "0";
            }

            var result = new Stack<char>();

            while (inputNumber != 0)
            {
                result.Push(Clistarr[inputNumber % 36]);
                inputNumber /= 36;
            }
            
            return new string(result.ToArray());
        }

        public static long Decode(string inputString)
        {
            long result = 0;
            var pow = 0;

            for (var i = inputString.Length - 1; i >= 0; i--)
            {
                var c = inputString[i];
                var pos = Clist.IndexOf(c);

                if (pos > -1)
                {
                    result += pos*(long)Math.Pow(Clist.Length, pow);
                }
                else
                {
                    return -1;
                }

                pow++;
            }
            return result;
        }

        public static string ToBase36(this long inputNumber)
        {
            return Encode(inputNumber);
        }

        public static string ToBase36(this ulong inputNumber)
        {
            return Encode(inputNumber);
        }

        public static string Encode(long inputNumber)
        {
            return Encode((ulong)inputNumber);
        }
    }
}
