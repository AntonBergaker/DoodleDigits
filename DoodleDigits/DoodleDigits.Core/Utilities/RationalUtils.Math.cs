using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using Rationals;

namespace DoodleDigits.Core.Utilities {
    public static partial class RationalUtils {
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

        private static readonly Dictionary<int, char> CharactersNumbers =
            NumberCharacters.ToDictionary(x => x.Value, x => x.Key);

        public static readonly Rational Tau = RationalUtils.Parse(
                "6.2831853071795864769252867665590057683943387987502116419498891846156328125724179972560696506842341359");

        public static readonly Rational Pi = Tau / 2;

        public static readonly Rational EulersNumber = RationalUtils.Parse(
                "2.7182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274");
        

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
