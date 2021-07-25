using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static partial class BinaryOperations {

        public static Value Xor(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is BooleanValue lhsBool && rhs is BooleanValue rhsBool) {
                return new BooleanValue(lhsBool.Value ^ rhsBool.Value);
            }

            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context, context.Node.Lhs.Position);
                rhsReal = rhsReal.Round(context, context.Node.Rhs.Position);

                return new RealValue(lhsReal.Value.Numerator ^ rhsReal.Value.Numerator);
            }

            return new UndefinedValue();
        }

        public static Value BitwiseOr(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context, context.Node.Lhs.Position);
                rhsReal = rhsReal.Round(context, context.Node.Rhs.Position);

                return new RealValue(lhsReal.Value.Numerator | rhsReal.Value.Numerator);
            }

            return new UndefinedValue();
        }

        public static Value BitwiseAnd(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context, context.Node.Lhs.Position);
                rhsReal = rhsReal.Round(context, context.Node.Rhs.Position);

                return new RealValue(lhsReal.Value.Numerator & rhsReal.Value.Numerator);
            }

            return new UndefinedValue();
        }

        public static Value ShiftLeft(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            {
                if (rhs is TooBigValue tbvRhs && lhs is RealValue realLhs) {
                    if (realLhs.Value.IsZero) {
                        return new RealValue(Rational.Zero);
                        ;
                    }

                    return tbvRhs.IsPositive
                        ? new TooBigValue(TooBigValue.Sign.Positive)
                        : new RealValue(Rational.Zero);
                    ;
                }
            }
            {
                if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                    var (realLhs, realRhs) = ConvertToReal(ctrLhs, ctrRhs, context);
                    realLhs = realLhs.Round(context, context.Node.Lhs.Position);
                    realRhs = realRhs.Round(context, context.Node.Rhs.Position);

                    if (realLhs.Value.IsZero) {
                        return new RealValue(Rational.Zero);
                    }

                    if (Rational.Abs(realRhs.Value) > 10000) {
                        return new TooBigValue(TooBigValue.Sign.Positive);
                    }

                    if (realRhs.Value < 0) {
                        return new RealValue(
                            RationalUtils.Floor(new Rational(
                                realLhs.Value.Numerator,
                                realRhs.Value.Denominator * BigInteger.Pow(2, -(int)realRhs.Value))
                            )
                        );
                    }
                    return new RealValue(RationalUtils.Floor(Rational.Pow(realLhs.Value, (int)realRhs.Value)));
                }
            }
            return new UndefinedValue();
        }

        public static Value ShiftRight(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (rhs is TooBigValue tbvRhs) {
                return ShiftLeft(lhs, tbvRhs.Negate(), context);
            }
            if (rhs is IConvertibleToReal ctrRhs) {
                var realRhs = ctrRhs.ConvertToReal(context, context.Node.Rhs.Position);
                return ShiftLeft(lhs, new RealValue(-realRhs.Value), context);
            }

            return new UndefinedValue();
        }

    }
}
