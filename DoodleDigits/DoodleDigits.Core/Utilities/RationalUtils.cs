using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using Rationals;

namespace DoodleDigits.Core.Utilities {
    public static class RationalUtils {
        private static readonly Dictionary<char, int> NumberCharacters = new() {
            { '0', 0 },
            { '1', 1 },
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'a', 10 },
            { 'b', 11 },
            { 'c', 12 },
            { 'd', 13 },
            { 'e', 14 },
            { 'f', 15 },
        };


        public static readonly Rational Tau = RationalUtils.Parse(
                "6.2831853071795864769252867665590057683943387987502116419498891846156328125724179972560696506842341359");

        public static readonly Rational Pi = Tau / 2;

        public static readonly Rational EulersNumber = RationalUtils.Parse(
                "2.7182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274");

        public static Rational Parse(string input, int maxMagnitude = 200, int @base = 10) {
            if (TryParse(input, out Rational result, maxMagnitude, @base) == false) {
                throw new FormatException("Input is not a parseable rational");
            }
            return result;
        }


        public static bool TryParse(string input, out Rational rational, int maxMagnitude = 200, int @base = 10) {
            return TryParseInternal(input, out rational, maxMagnitude, @base, true);
        }

        public static bool TryParseInternal(string input, out Rational rational, int maxMagnitude, int @base, bool tryScientific) {
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

                    if (NumberCharacters.TryGetValue(@char, out int value)) {
                        if (value >= @base) {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }

                if (BigInteger.TryParse(numeratorString.ToString(), @base == 16 ? NumberStyles.HexNumber : NumberStyles.Number, default, out BigInteger numerator) == false) {
                    return false;
                }

                BigInteger denominator = denominatorMagnitude == 1
                    ? BigInteger.One
                    : BigInteger.Pow(@base, denominatorMagnitude-1);

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
            } else {
                rational = preEValue * BigInteger.Pow(10, postEValue);
            }

            return true;
        }

        public static string ToDecimalString(this Rational value, int maximumDecimals = 30) {
            value = value.CanonicalForm;
            StringBuilder sb = new StringBuilder(maximumDecimals);

            if (value.Numerator < 0) {
                sb.Append('-');
            }

            int magnitude = value.Magnitude;
            if (magnitude < 0) {
                sb.Append('0');
                sb.Append('.');
                sb.Append('0', -magnitude-1);
            }

            var enumerator = value.Digits;
            int index = 0;
            foreach (char c in enumerator) {
                if (index > 0 && index == magnitude+1) {
                    sb.Append(".");
                }

                if (index > magnitude && index - magnitude > maximumDecimals) {
                    sb.Append("...");
                    break;
                }

                sb.Append(c);

                index++;
            }

            // Add missing 0s
            int remaining0s = magnitude - (index-1);
            if (remaining0s > 0) {
                sb.Append('0', remaining0s);
            }

            return sb.ToString();

        }

        public static string ToScientificString(this Rational value, int decimals = 10, string exponentCharacter = "E") {
            value = value.CanonicalForm;
            if (value.IsZero) {
                return "0";
            }

            int magnitude = value.Magnitude;

            StringBuilder sb = new StringBuilder(decimals);
            if (value.Numerator < 0) {
                sb.Append('-');
            }

            var enumerator = value.Digits;
            int index = 0;
            foreach (char c in enumerator) {
                if (index == 1) {
                    sb.Append(".");
                }
                sb.Append(c);
                index++;
                if (index > decimals) {
                    break;
                }

            }

            return $"{sb}{exponentCharacter}{magnitude}";
        }

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
        /// Approximates given double number as a rational number. If a higher tolerance is given, simpler double might be returned.
        /// </summary>
        /// <param name="input">Input double</param>
        /// <param name="tolerance">Optional tolerance</param>
        /// <returns>Output rational</returns>
        public static Rational FromDouble(double input, double tolerance = 0) {
            if (tolerance < 0) throw new ArgumentOutOfRangeException(nameof(tolerance));
            // Lifted from https://github.com/tompazourek/Rationals/tree/master/src/Rationals made to work with doubles
            var continuedFraction = ExpandToContinuedFraction(input);

            var sequence = new List<BigInteger>();
            var previousDifference = double.MaxValue;
            var currentNumber = Rational.Zero;
            foreach (var coefficient in continuedFraction) {
                sequence.Add(coefficient);
                currentNumber = Rational.FromContinuedFraction(sequence);
                var currentDifference = Math.Abs(currentNumber.ToDouble() - input);
                if (currentDifference <= tolerance) {
                    break;
                }
                if (currentDifference < previousDifference) {
                    previousDifference = currentDifference;
                } else {
                    break;
                }
            }
            return currentNumber;
        } private static IEnumerable<BigInteger> ExpandToContinuedFraction(double d) {
            // Lifted from https://github.com/tompazourek/Rationals/tree/master/src/Rationals made to work with doubles
            var wholePart = Math.Truncate(d);
            var fractionPart = d - wholePart;
            yield return (BigInteger)wholePart;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            while (fractionPart != 0) {
                d = 1d / fractionPart;

                wholePart = Math.Truncate(d);
                fractionPart = d - wholePart;
                yield return (BigInteger)wholePart;
            }
        }


        public static readonly Rational Half = new Rational(BigInteger.One, 2);

        public static Rational Floor(Rational value) {
            return value.WholePart;
        }

        public static Rational Round(Rational value) {
            BigInteger whole = value.WholePart;
            if (value.FractionPart > RationalUtils.Half) {
                whole += 1;
            }
            return whole;
        }

        public static Rational Ceil(Rational value) {
            BigInteger whole = value.WholePart;
            if (value.FractionPart > 0) {
                whole += 1;
            }
            return whole;
        }

        public static Rational Modulus(this Rational @this, Rational divisor) {
            Rational divided = @this / divisor;
            Rational floored = Floor(divided);

            return (divided - floored) * divisor;
        } 

    }
}
