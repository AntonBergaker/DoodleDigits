using System;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public abstract class Value : IEquatable<Value> {
        public abstract override string ToString();

        public static Value FromDouble(double value, bool resultOfInfinity = false) {
            if (double.IsPositiveInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.PositiveInfinity : TooBigValue.Sign.Positive);
            }

            if (double.IsNegativeInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.NegativeInfinity : TooBigValue.Sign.Negative);
            }

            if (double.IsNaN(value)) {
                return new UndefinedValue();
            }

            return new RealValue((Rational) value);
        }

        public abstract bool Equals(Value? other);

        public override bool Equals(object? obj) {
            if (obj is not Value val) {
                return false;
            }
            return Equals(val);
        }

        public abstract override int GetHashCode();
    }
}
