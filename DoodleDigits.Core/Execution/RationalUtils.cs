using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rationals;

namespace DoodleDigits.Core.Execution {
    static class RationalUtils {
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

            rational = new Rational(numerator, denominator);
            return true;
        }

    }
}
