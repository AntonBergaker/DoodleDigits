using System;
using System.Numerics;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static partial class BinaryOperations {
        private static (RealValue lhs, RealValue rhs) ConvertToReal(IConvertibleToReal lhs, IConvertibleToReal rhs,
            ExecutionContext<BinaryOperation> context) {
            return (
                lhs.ConvertToReal(context.ForNode(context.Node.Lhs)),
                rhs.ConvertToReal(context.ForNode(context.Node.Rhs))
            );
        }

        public static Value Add(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new RealValue((result.lhs.Value + result.rhs.Value).CanonicalForm);
        }

        public static Value Subtract(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue tbvRhs) {
                return tbvRhs.Negate();
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new RealValue((result.lhs.Value - result.rhs.Value).CanonicalForm);
        }


        public static Value Divide(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                int sign = (tbLhs.IsPositive ? 1 : -1) * (tbRhs.IsPositive ? 1 : -1);
                return sign == 1 ? tbLhs : tbLhs.Negate();
            }

            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return new RealValue(0);
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            if (result.rhs.Value == 0) {
                return new UndefinedValue();
            }

            return new RealValue((result.lhs.Value / result.rhs.Value).CanonicalForm);
        }

        public static Value Multiply(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            // Undefined check, undefined times undefined is undefined
            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue();
            }

            // 0 checks, 0's always result in 0s
            {
                if (lhs is RealValue rLhs && rLhs.Value == 0) {
                    return rLhs;
                }

                if (rhs is RealValue rRhs && rRhs.Value == 0) {
                    return rRhs;
                }
            }

            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                int sign = (tbLhs.IsPositive ? 1 : -1) * (tbRhs.IsPositive ? 1 : -1);
                return sign == 1 ? tbLhs : tbLhs.Negate();
            }

            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new RealValue((result.lhs.Value * result.rhs.Value).CanonicalForm);
        }


        public static Value Modulus(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return new UndefinedValue();
            }

            if (rhs is TooBigValue) {
                return lhs;
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            if (result.rhs.Value == 0) {
                return new UndefinedValue();
            }


            return new RealValue(result.lhs.Value.Modulus(result.rhs.Value).CanonicalForm);
        }


        public static Value Power(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var (realValue0, realValue1) = ConvertToReal(ctrLhs, ctrRhs, context);

            return Power(realValue0, realValue1);
        }

        public static Value Power(RealValue lhs, RealValue rhs) {
            if (lhs.Value.IsZero && rhs.Value < Rational.Zero) {
                return new UndefinedValue();
            }

            if (lhs.Value.IsZero) {
                return new RealValue(Rational.Zero);
            }
            if (rhs.Value.IsZero) {
                return new RealValue(Rational.One);
            }

            if (rhs.HasDecimal == false && rhs.Value < 10000) {
                int lhsMagnitude = lhs.Value.Magnitude;
                if (Rational.Abs((lhsMagnitude + 1) * rhs.Value) > 10000) {
                    return new TooBigValue(TooBigValue.Sign.Positive);
                }

                return new RealValue(Rational.Pow(lhs.Value, (int)rhs.Value).CanonicalForm);
            }

            /*if (lhs.Value > RationalUtils.MaxDouble || rhs.Value > RationalUtils.MaxDouble) {
                return new TooBigValue(TooBigValue.Sign.Positive);
            }*/
            return Value.FromDouble(Math.Pow(lhs.Value.ToDouble(), rhs.Value.ToDouble()));
        }


    }
}
