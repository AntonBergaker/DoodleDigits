using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rationals;

namespace DoodleDigits.Core.Utilities; 
public static partial class RationalUtils {

    public static Rational Parse(string input, int maxMagnitude = 200, int @base = 10) {
        if (TryParse(input, out Rational result, maxMagnitude, @base) == false) {
            throw new FormatException("Input is not a parseable rational");
        }

        return result;
    }


    public static bool TryParse(string input, out Rational rational, int maxMagnitude = 200, int @base = 10) {
        return TryParseInternal(input, out rational, maxMagnitude, @base, true);
    }

    public static bool TryParseInternal(string input, out Rational rational, int maxMagnitude, int @base,
        bool tryScientific) {
        if (@base == 10 && TryParseScientific(input, maxMagnitude, out Rational scientificRational)) {
            rational = scientificRational;
            return true;
        }

        // Set rational so we don't have to do it at every early return
        rational = default;

        // For base 16 and base 10, we can use BigInteger parsing for a good speed increase
        if (@base == 16 || @base == 10) {
            StringBuilder numeratorString = new();
            int denominatorMagnitude = 1;
            bool passedDecimal = false;

            if (input.StartsWith("-")) {
                input = input[1..];
                numeratorString.Append('-');
            }

            // Leading 0 because of biginteger weirdness
            numeratorString.Append("0");

            foreach (char @char in input) {
                if (@char == '_' || @char == ' ') {
                    continue;
                }

                if (@char == '.') {
                    if (passedDecimal) {
                        return false;
                    }

                    passedDecimal = true;
                    continue;
                }

                numeratorString.Append(@char);
                if (passedDecimal) {
                    denominatorMagnitude += 1;
                }

                if (NumberCharacters.TryGetValue(char.ToLowerInvariant(@char), out int value)) {
                    if (value >= @base) {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }

            // Leading 0 because of biginteger weirdness
            if (BigInteger.TryParse(numeratorString.ToString(),
                    @base == 16 ? NumberStyles.HexNumber : NumberStyles.Number, default,
                    out BigInteger numerator)
                == false) {
                return false;
            }

            BigInteger denominator = denominatorMagnitude == 1
                ? BigInteger.One
                : BigInteger.Pow(@base, denominatorMagnitude - 1);

            rational = new Rational(numerator, denominator).CanonicalForm;

            return true;
        }
        else {
            BigInteger numerator = 0;
            BigInteger denominator = 1;

            if (@base <= 1 || @base > 16) {
                throw new ArgumentOutOfRangeException(nameof(@base));
            }

            if (input.StartsWith("-")) {
                input = input[1..];
                denominator = -denominator;
            }

            bool passedDecimal = false;

            foreach (char @char in input) {
                if (@char == '_' || @char == ' ') {
                    continue;
                }

                if (@char == '.') {
                    if (passedDecimal) {
                        return false;
                    }

                    passedDecimal = true;
                    continue;
                }

                numerator *= @base;
                if (passedDecimal) {
                    denominator *= @base;
                }

                if (NumberCharacters.TryGetValue(@char, out int value)) {
                    if (value >= @base) {
                        return false;
                    }

                    numerator += value;
                }
                else {
                    return false;
                }
            }

            rational = new Rational(numerator, denominator).CanonicalForm;
            return true;
        }
    }



    private static bool TryParseScientific(string input, int maxMagnitude, out Rational rational) {
        bool hasDigit = false;
        bool isNegative = false;
        int eIndex = -1;

        // Check that it ends on a whole number + E
        for (int i = input.Length - 1; i >= 0; i--) {
            char c = input[i];

            if (c is 'E' or 'e' or 'ᴇ') {
                eIndex = i;
                break;
            }

            if (c == ' ') {
                continue;
            }

            // If we turned negative, it has to be a e before it
            if (isNegative) {
                rational = default;
                return false;
            }

            if (char.IsDigit(c)) {
                hasDigit = true;
                continue;
            }

            if (c == '-') {
                isNegative = true;
                continue;
            }
        }

        if (eIndex == -1 || hasDigit == false) {
            rational = default;
            return false;
        }

        if (TryParseInternal(input[..(eIndex)], out Rational preEValue, 100, 10, false) == false) {
            rational = default;
            return false;
        }

        if (int.TryParse(input[(eIndex + 1)..], out int postEValue) == false) {
            rational = default;
            return false;
        }

        if (Math.Abs(postEValue) > maxMagnitude) {
            rational = default;
            return false;
        }

        if (postEValue < 0) {
            rational = preEValue * new Rational(1, BigInteger.Pow(10, -postEValue));
        }
        else {
            rational = preEValue * BigInteger.Pow(10, postEValue);
        }

        return true;
    }


    public static string ToDecimalString(this Rational value, int maximumDecimals = 30, int @base = 10) {
        value = value.CanonicalForm;
        StringBuilder sb = new StringBuilder(maximumDecimals);

        if (value.Numerator < 0) {
            sb.Append('-');
        }

        int magnitude = value.Magnitude(@base);
        if (magnitude < 0) {
            sb.Append('0');
            sb.Append('.');
            sb.Append('0', -magnitude - 1);
        }

        var enumerator = value.Digits(@base);
        int index = 0;
        foreach (char c in enumerator) {
            if (index > 0 && index == magnitude + 1) {
                sb.Append('.');
            }

            if (index > magnitude && index - magnitude > maximumDecimals) {
                sb.Append("...");
                break;
            }

            sb.Append(c);

            index++;
        }

        // Add missing 0s
        int remaining0S = magnitude - (index - 1);
        if (remaining0S > 0) {
            sb.Append('0', remaining0S);
        }

        return sb.ToString();

    }

    public static string ToScientificString(this Rational value, int decimals = 10, int @base = 10, string exponentCharacter = "E") {
        value = value.CanonicalForm;
        if (value.IsZero) {
            return "0";
        }

        int magnitude = value.Magnitude(@base);

        StringBuilder sb = new StringBuilder(decimals);
        if (value.Numerator < 0) {
            sb.Append('-');
        }

        var enumerator = value.Digits(@base);
        int index = 0;
        foreach (char c in enumerator) {
            if (index == 1) {
                sb.Append('.');
            }

            sb.Append(c);
            index++;
            if (index > decimals) {
                break;
            }

        }

        return $"{sb}{exponentCharacter}{magnitude}";
    }

    /// <summary>
    /// Enumerates significant digits of the rational number with the given base
    /// Omits leading and trailing zeros (only exception is number zero).
    /// </summary>
    /// Copied straight from the rational library with base changed
    public static IEnumerable<char> Digits(this Rational rational, int @base) {
        var numerator = BigInteger.Abs(rational.Numerator);
        var denominator = BigInteger.Abs(rational.Denominator);
        var removeLeadingZeros = true;
        var returnedAny = false;
        while (numerator > 0) {
            var divided = BigInteger.DivRem(numerator, denominator, out var rem);

            var digits = divided.ToStringBase(@base);

            if (rem == 0)
                digits = digits.TrimEnd('0'); // remove trailing zeros

            foreach (var digit in digits) {
                if (removeLeadingZeros && digit == '0')
                    continue;

                yield return digit;
                removeLeadingZeros = false;
                returnedAny = true;
            }

            numerator = rem * @base;
        }

        if (!returnedAny)
            yield return '0';
    }

    private static string ToStringBase(this BigInteger integer, int @base) {
        if (@base == 10) {
            return integer.ToString(CultureInfo.InvariantCulture);
        }
        if (@base == 16) {
            return integer.ToString("X", CultureInfo.InvariantCulture);
        }

        StringBuilder sb = new StringBuilder();
        while (integer > 0) {
            var divided= BigInteger.DivRem(integer, @base, out var rem);

            sb.Insert(0, CharactersNumbers[(int)rem]);

            integer = divided;
        }

        return sb.ToString();
    }

}