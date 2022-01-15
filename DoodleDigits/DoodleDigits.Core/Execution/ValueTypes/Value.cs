using System;
using System.Diagnostics.CodeAnalysis;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public abstract class Value : IEquatable<Value> {
        public abstract override string ToString();

        public bool TriviallyAchieved { get; }

        protected Value(bool triviallyAchieved) {
            TriviallyAchieved = triviallyAchieved;
        }

        public static Value FromDouble(double value, bool triviallyAchieved, RealValue.PresentedForm form, bool resultOfInfinity = false) {
            if (double.IsPositiveInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.PositiveInfinity : TooBigValue.Sign.Positive);
            }

            if (double.IsNegativeInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.NegativeInfinity : TooBigValue.Sign.Negative);
            }

            if (double.IsNaN(value)) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new RealValue(RationalUtils.FromDouble(value), triviallyAchieved, form);
        }

        public virtual Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool castAttempt, ExecutionContext<BinaryOperation> context) {
            return null;
        }

        public virtual Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool castAttempt, ExecutionContext<BinaryOperation> context) {
            return null;
        }



        public abstract bool Equals(Value? other);

        public override bool Equals(object? obj) {
            if (obj is not Value val) {
                return false;
            }
            return Equals(val);
        }

        public abstract override int GetHashCode();

        public abstract Value Clone(bool? triviallyAchieved = null);
    }
}
