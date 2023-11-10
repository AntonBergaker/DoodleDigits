using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rationals;

namespace DoodleDigits.Core.Utilities; 
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

    /// <summary>
    /// Returns the number of digits of the given number
    /// </summary>
    public static int GetNumberOfDigits(BigInteger x, int @base) {
        x = BigInteger.Abs(x);

        var digits = 0;
        while (x > 0) {
            digits++;
            x /= @base;
        }

        return digits;
    }

    /// <summary>
    /// Gets the magnitude for a given base
    /// </summary>
    /// <param name="rational"></param>
    /// <param name="base"></param>
    /// <returns></returns>
    /// Copied from the rational library with base changed
    public static int Magnitude(this Rational rational, int @base) {
        if (rational.IsZero)
            return 0;

        // thanks to 0jpq0 for this magnitude algorithm
        // https://github.com/tompazourek/Rationals/issues/20#issue-398771661

        var numeratorDigits = GetNumberOfDigits(rational.Numerator, @base);
        var denominatorDigits = GetNumberOfDigits(rational.Denominator, @base);

        var magnitude = numeratorDigits - denominatorDigits;

        var numeratorAbs = BigInteger.Abs(rational.Numerator);
        var denominatorAbs = BigInteger.Abs(rational.Denominator);

        if (numeratorDigits > denominatorDigits) {
            denominatorAbs *= BigInteger.Pow(@base, numeratorDigits - denominatorDigits);
        } else if (numeratorDigits < denominatorDigits) {
            numeratorAbs *= BigInteger.Pow(@base, denominatorDigits - numeratorDigits);
        }

        if (numeratorAbs < denominatorAbs) {
            magnitude--;
        }

        return magnitude;
    }
}
