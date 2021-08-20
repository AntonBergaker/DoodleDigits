using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rationals;

namespace DoodleDigits.Core.Utilities {
    partial class RationalUtils {
        public static long GetComplexity(this BigInteger bigInteger) {
            return bigInteger.GetBitLength();
        }

        public static long GetComplexity(this Rational rational) {
            return Math.Max(rational.Numerator.GetComplexity(), rational.Denominator.GetComplexity());
        }

        /// <summary>
        /// Returns a rough estimate of the magnitude that's fast to calculate
        /// </summary>
        /// <param name="rational"></param>
        /// <returns></returns>
        public static int RoughMagnitude(this Rational rational) {
            long numeratorBitLength = rational.Numerator.GetBitLength();
            long denominatorBitLength = rational.Denominator.GetBitLength();

            long diff = numeratorBitLength - denominatorBitLength;
            return (int)(diff / 3.32192809488D);
        }
    }
}
