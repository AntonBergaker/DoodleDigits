using System.Numerics;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary;
public static partial class BinaryOperations {

    public static Value Xor(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
        if (lhs is BooleanValue lhsBool && rhs is BooleanValue rhsBool) {
            return new BooleanValue(lhsBool.Value ^ rhsBool.Value);
        }

        if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
            var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context, nodes);
            lhsReal = lhsReal.Round(context, nodes.Rhs);
            rhsReal = rhsReal.Round(context, nodes.Lhs);

            return new RealValue(lhsReal.Value.Numerator ^ rhsReal.Value.Numerator, false, lhsReal.Form);
        }
        
        if (lhs is UndefinedValue || rhs is UndefinedValue) {
            return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    public static Value BitwiseOr(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
        if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
            var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context, nodes);
            lhsReal = lhsReal.Round(context, nodes.Rhs);
            rhsReal = rhsReal.Round(context, nodes.Lhs);

            return new RealValue(lhsReal.Value.Numerator | rhsReal.Value.Numerator, false, lhsReal.Form);
        }

        if (lhs is UndefinedValue || rhs is UndefinedValue) {
            return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    public static Value BitwiseAnd(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
        if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
            var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context, nodes);
            lhsReal = lhsReal.Round(context, nodes.Rhs);
            rhsReal = rhsReal.Round(context, nodes.Lhs);

            return new RealValue(lhsReal.Value.Numerator & rhsReal.Value.Numerator, false, lhsReal.Form);
        }

        if (lhs is UndefinedValue || rhs is UndefinedValue) {
            return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    public static Value ShiftLeft(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
        if (lhs is TooBigValue) {
            return lhs;
        }

        {
            if (rhs is TooBigValue tbvRhs && lhs is RealValue realLhs) {
                if (realLhs.Value.IsZero) {
                    return new RealValue(Rational.Zero, false, realLhs.Form);
                }

                return tbvRhs.IsPositive
                    ? new TooBigValue(TooBigValue.Sign.Positive)
                    : new RealValue(Rational.Zero, false, realLhs.Form);
                ;
            }
        }
        {
            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var (lhsReal, rhsReal) = ConvertToReal(ctrLhs, ctrRhs, context, nodes);
                lhsReal = lhsReal.Round(context, nodes.Rhs);
                rhsReal = rhsReal.Round(context, nodes.Lhs);

                if (lhsReal.Value.IsZero) {
                    return new RealValue(Rational.Zero, false, lhsReal.Form);
                }

                if (Rational.Abs(rhsReal.Value) > 10000) {
                    return new TooBigValue(TooBigValue.Sign.Positive);
                }

                if (rhsReal.Value < 0) {
                    return new RealValue(
                        RationalUtils.Floor(new Rational(
                            lhsReal.Value.Numerator,
                            rhsReal.Value.Denominator * BigInteger.Pow(2, -(int)rhsReal.Value))
                        ), false, lhsReal.Form
                    );
                }
                return new RealValue(RationalUtils.Floor(lhsReal.Value * Rational.Pow(2, (int)rhsReal.Value)), false, lhsReal.Form);
            }
        }

        if (lhs is UndefinedValue || rhs is UndefinedValue) {
            return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    public static Value ShiftRight(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
        if (rhs is TooBigValue tbvRhs) {
            return ShiftLeft(lhs, tbvRhs.Negate(), context, nodes);
        }
        if (rhs is IConvertibleToReal ctrRhs) {
            var realRhs = ctrRhs.ConvertToReal(context, nodes.Rhs);
            return ShiftLeft(lhs, new RealValue(-realRhs.Value), context, nodes);
        }

        if (lhs is UndefinedValue || rhs is UndefinedValue) {
            return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

}
