using System;
using System.ComponentModel;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Functions.Implementations.Binary;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class TooBigValue {
    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert,
        ExecutionContext context, BinaryNodes nodes) {
        if (other is TooBigValue otherTbv) {
            if (this.ValueSign is Sign.Positive or Sign.Negative &&
                otherTbv.ValueSign is Sign.NegativeInfinity or Sign.PositiveInfinity) {
                return otherTbv;
            }

            return this;
        }

        if (shouldConvert && other is IConvertibleToReal) {
            // Anything we convert to is smaller than this so just return this
            return this;
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (other is TooBigValue) {
            int mySimpleSize = this.GetSimplifiedSize();
            int otherSimpleSize = this.GetSimplifiedSize();

            // Multiply by themselves so doing 2-1 does not put it into 1 range
            int total = (mySimpleSize * mySimpleSize) - (otherSimpleSize * otherSimpleSize);
            // Flip if we're right
            if (side == BinaryOperation.OperationSide.Right) {
                total = -total;
            }

            if (total == 0) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new TooBigValue(total switch {
                    1 => Sign.Positive,
                    >= 2 => Sign.PositiveInfinity,
                    -1 => Sign.Negative,
                    <= -2 => Sign.NegativeInfinity,
                    0 => throw new Exception(),
                }
                , false);
        }

        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var _)) {
            // Anything we convert to is smaller than this so just return this

            // Negative if we're on the right side
            if (side == BinaryOperation.OperationSide.Right) {
                return this.Negate();
            }

            return this;
        }

        return null;
    }

    public override Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (other is TooBigValue otherTooBigValue) {
                int lhsSimpleSize = this.GetSimplifiedSize();
                int rhsSimpleSize = otherTooBigValue.GetSimplifiedSize();
                if (Math.Abs(lhsSimpleSize) > Math.Abs(rhsSimpleSize)) {
                    int sign = Math.Sign(lhsSimpleSize * rhsSimpleSize);
                    return new TooBigValue(sign == -1 ? Sign.NegativeInfinity : Sign.PositiveInfinity);
                }

                if (Math.Abs(lhsSimpleSize) < Math.Abs(rhsSimpleSize)) {
                    return new RealValue(Rational.Zero, false, RealValue.PresentedForm.Unset);
                }

                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            // Infinity divided by anything except 0 is infinity
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                if (otherReal.Value.IsZero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return this;
            }

        }
        if (side == BinaryOperation.OperationSide.Right) {
            // Interactions with tbv handled in left already
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                return new RealValue(Rational.Zero, false, otherReal.Form);
            }
        }

        return null;
    }

    public override Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (other is TooBigValue otherTbv) {
            int mySize = this.GetSimplifiedSize();
            int otherSize = otherTbv.GetSimplifiedSize();

            return new TooBigValue((mySize * otherSize) switch {
                    1 => Sign.Positive,
                    >=2 => Sign.PositiveInfinity,
                    -1 => Sign.Negative,
                    <=-2 => Sign.NegativeInfinity,

                    0 => throw new InvalidOperationException(),
                }
                , false);
        }

        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            if (otherRealValue.Value.IsZero) {
                return otherRealValue;
            }

            if (otherRealValue.Value < Rational.Zero) {
                return this.Negate();
            }

            return this;
        }

        return null;
    }

    public override Value? TryModulus(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (other is TooBigValue otherTbv) {
                int mySize = this.GetSimplifiedSize();
                int otherSize = otherTbv.GetSimplifiedSize();

                if (Math.Abs(otherSize) > Math.Abs(mySize)) {
                    return this;
                }

                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }
        }

        if (side == BinaryOperation.OperationSide.Right) {
            // other tbv handled in left

            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                return otherReal;
            }
        }

        return null;
    }

    public override Value? TryPower(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (other is TooBigValue) {
                if (GetSimplifiedSize() < 0) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Error);
                }

                return other;
            }

            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                if (otherReal.Value.IsZero) {
                    return new RealValue(Rational.One, false, otherReal.Form);
                }

                if (otherReal.Value < Rational.Zero) {
                    return new RealValue(Rational.Zero, false, otherReal.Form);
                }

                return this;
            }
        }

        if (side == BinaryOperation.OperationSide.Right) {
            // other tbv handled in left
            
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                if (otherReal.Value.IsOne) {
                    return new RealValue(Rational.One, false, otherReal.Form);
                }

                if (otherReal.Value < Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                if (otherReal.Value < Rational.One) {
                    return new RealValue(Rational.Zero, false, otherReal.Form);
                }

                return this;
            }
        }

        return null;
    }
}