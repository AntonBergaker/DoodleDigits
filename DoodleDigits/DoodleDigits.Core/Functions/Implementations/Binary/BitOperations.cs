using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary {
    public static partial class BinaryOperations {

        public static Value Xor(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is BooleanValue lhsBool && rhs is BooleanValue rhsBool) {
                return new BooleanValue(lhsBool.Value ^ rhsBool.Value);
            }

            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context.ForNode(context.Node.Rhs));
                rhsReal = rhsReal.Round(context.ForNode(context.Node.Lhs));

                return new RealValue(lhsReal.Value.Numerator ^ rhsReal.Value.Numerator, false, lhsReal.Form, context.Node);
            }
            
            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        public static Value BitwiseOr(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context.ForNode(context.Node.Rhs));
                rhsReal = rhsReal.Round(context.ForNode(context.Node.Lhs));

                return new RealValue(lhsReal.Value.Numerator | rhsReal.Value.Numerator, false, lhsReal.Form, context.Node);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        public static Value BitwiseAnd(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                var (lhsReal, rhsReal) = ConvertToReal(lhsCtr, rhsCtr, context);
                lhsReal = lhsReal.Round(context.ForNode(context.Node.Rhs));
                rhsReal = rhsReal.Round(context.ForNode(context.Node.Lhs));

                return new RealValue(lhsReal.Value.Numerator & rhsReal.Value.Numerator, false, lhsReal.Form, context.Node);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        public static Value ShiftLeft(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            {
                if (rhs is TooBigValue tbvRhs && lhs is RealValue realLhs) {
                    if (realLhs.Value.IsZero) {
                        return new RealValue(Rational.Zero, false, realLhs.Form, context.Node);
                    }

                    return tbvRhs.IsPositive
                        ? new TooBigValue(TooBigValue.Sign.Positive)
                        : new RealValue(Rational.Zero, false, realLhs.Form, context.Node);
                    ;
                }
            }
            {
                if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                    var (lhsReal, rhsReal) = ConvertToReal(ctrLhs, ctrRhs, context);
                    lhsReal = lhsReal.Round(context.ForNode(context.Node.Rhs));
                    rhsReal = rhsReal.Round(context.ForNode(context.Node.Lhs));

                    if (lhsReal.Value.IsZero) {
                        return new RealValue(Rational.Zero, false, lhsReal.Form, context.Node);
                    }

                    if (Rational.Abs(rhsReal.Value) > 10000) {
                        return new TooBigValue(TooBigValue.Sign.Positive);
                    }

                    if (rhsReal.Value < 0) {
                        return new RealValue(
                            RationalUtils.Floor(new Rational(
                                lhsReal.Value.Numerator,
                                rhsReal.Value.Denominator * BigInteger.Pow(2, -(int)rhsReal.Value))
                            ), false, lhsReal.Form, context.Node
                        );
                    }
                    return new RealValue(RationalUtils.Floor(lhsReal.Value * Rational.Pow(2, (int)rhsReal.Value)), false, lhsReal.Form, context.Node);
                }
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        public static Value ShiftRight(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (rhs is TooBigValue tbvRhs) {
                return ShiftLeft(lhs, tbvRhs.Negate(), context);
            }
            if (rhs is IConvertibleToReal ctrRhs) {
                var realRhs = ctrRhs.ConvertToReal(context.ForNode(context.Node.Rhs));
                return ShiftLeft(lhs, new RealValue(-realRhs.Value), context);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

    }
}
