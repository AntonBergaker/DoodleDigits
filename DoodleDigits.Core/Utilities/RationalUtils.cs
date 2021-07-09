using System;
using System.Collections.Generic;
using System.Numerics;
using Rationals;

namespace DoodleDigits.Core.Utilities {
    public static class RationalUtils {
        private static readonly Dictionary<char, int> NumberCharacters = new() {
            {'0', 0},
            {'1', 1},
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'a', 10},
            {'b', 11},
            {'c', 12},
            {'d', 13},
            {'e', 14},
            {'f', 15},
        };

        public static bool TryParse(string input, out Rational rational, int @base = 10) {
            BigInteger numerator = 0;
            BigInteger denominator = 1;

            if (@base <= 1 || @base > 16) {
                throw new ArgumentOutOfRangeException(nameof(@base));
            }

            bool passedDecimal = false;

            foreach (char @char in input) {
                if (@char == '_' || @char == ' ') {
                    continue;
                }

                if (@char == '.') {
                    if (passedDecimal) {
                        rational = 0;
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
                        rational = 0;
                        return false;
                    }
                    numerator += value;
                }
                else {
                    rational = 0;
                    return false;
                }
            }

            rational = new Rational(numerator, denominator).CanonicalForm;
            return true;
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
