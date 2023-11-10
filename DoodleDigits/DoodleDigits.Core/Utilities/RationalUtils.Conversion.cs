using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rationals;

namespace DoodleDigits.Core.Utilities; 
public static partial class RationalUtils {
    public static double ToDouble(this Rational rational) {
        // Lifted from https://github.com/tompazourek/Rationals/tree/master/src/Rationals made to work with doubles
        if (rational < 0)
            return -ToDouble(-rational);

        double result = 0;
        var numerator = rational.Numerator;
        var denominator = rational.Denominator;
        var scale = 1D;
        var previousScale = 0D;
        while (numerator != 0) {
            var divided = BigInteger.DivRem(numerator, denominator, out var rem);

            if (scale == 0) {
                if (divided >= 5)
                    result += previousScale; // round up last digit

                break;
            }

            result += (double)divided * scale;

            numerator = rem * 10;
            previousScale = scale;
            scale /= 10;
        }

        return result;
    }

    /// <summary>
    /// Returns provided double as a rational
    /// </summary>
    /// <param name="input">Input double</param>
    /// <returns>Output rational</returns>
    public static Rational FromDouble(double input) {
        if (double.IsNaN(input) || double.IsInfinity(input)) {
            throw new Exception($"Double {input} can not be expressed as a rational");
        }

        long bits = BitConverter.DoubleToInt64Bits(input);

        int sign = (int)(bits >> 63);
        int exponent =  (int)((bits & 0b01111111_11110000_00000000_00000000_00000000_00000000_00000000_00000000) >> 52);
        long mantissa = (long)(bits & 0b00000000_00001111_11111111_11111111_11111111_11111111_11111111_11111111);

        if (mantissa == 0 && exponent == 0) {
            return Rational.Zero;
        }

        // Relocate exponent to proper value, 1023 for how doubles work and 52 for the denominator if the mantissa was expressed as a rational
        exponent = exponent - 1023 - 52;
        
        // Add the leading 1 to mantissa
        long numerator = mantissa | 0b00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000;

        // Shorten num as far as possible, adjusting the exponent
        while ((numerator & 1) == 0) {
            numerator >>= 1;
            exponent++;
        }

        if (sign != 0) {
            numerator = -numerator;
        }

        BigInteger bigIntegerNumerator;
        BigInteger bigIntegerDenominator;

        if (exponent >= 0) {
            bigIntegerNumerator = (BigInteger)numerator << exponent;
            bigIntegerDenominator = BigInteger.One;
        }
        else {
            bigIntegerDenominator = BigInteger.One << -exponent;
            bigIntegerNumerator = numerator;
        }

        return new Rational(bigIntegerNumerator, bigIntegerDenominator);
    }
}
