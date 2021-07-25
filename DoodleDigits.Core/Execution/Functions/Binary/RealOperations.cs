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
            return (lhs.ConvertToReal(context, context.Node.Lhs.Position),
                rhs.ConvertToReal(context, context.Node.Rhs.Position));
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
                return new TooBigValue(result.lhs.Value > 0
                    ? TooBigValue.Sign.PositiveInfinity
                    : TooBigValue.Sign.NegativeInfinity);
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

            if (realValue0.Value == 0 && realValue1.Value < 0) {
                return new UndefinedValue();
            }

            if (realValue1.HasDecimal == false &&
                Rational.Abs((realValue0.Value.Magnitude + 1) * realValue1.Value) < 10000) {
                return new RealValue(Rational.Pow(realValue0.Value, (int) realValue1.Value));
            }

            return Value.FromDouble(Math.Pow((double) realValue0.Value, (double) realValue1.Value));
        }

        public static Value LessThan(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                return new BooleanValue(tbLhs.GetSimplifiedSize() < tbRhs.GetSimplifiedSize());
            }

            if (lhs is TooBigValue) {
                return new BooleanValue(false);
            }

            if (rhs is TooBigValue) {
                return new BooleanValue(true);
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new BooleanValue(result.lhs.Value < result.rhs.Value);
        }

        public static Value LessOrEqualTo(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                return new BooleanValue(tbLhs.GetSimplifiedSize() <= tbRhs.GetSimplifiedSize());
            }

            if (lhs is TooBigValue) {
                return new BooleanValue(false);
            }

            if (rhs is TooBigValue) {
                return new BooleanValue(true);
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new BooleanValue(result.lhs.Value <= result.rhs.Value);
        }

        public static Value GreaterThan(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                return new BooleanValue(tbLhs.GetSimplifiedSize() > tbRhs.GetSimplifiedSize());
            }

            if (lhs is TooBigValue) {
                return new BooleanValue(true);
            }

            if (rhs is TooBigValue) {
                return new BooleanValue(false);
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new BooleanValue(result.lhs.Value > result.rhs.Value);
        }

        public static Value GreaterOrEqualTo(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                return new BooleanValue(tbLhs.GetSimplifiedSize() >= tbRhs.GetSimplifiedSize());
            }

            if (lhs is TooBigValue) {
                return new BooleanValue(true);
            }

            if (rhs is TooBigValue) {
                return new BooleanValue(false);
            }

            if (lhs is not IConvertibleToReal ctrLhs || rhs is not IConvertibleToReal ctrRhs) {
                return new UndefinedValue();
            }

            var result = ConvertToReal(ctrLhs, ctrRhs, context);
            return new BooleanValue(result.lhs.Value >= result.rhs.Value);
        }

    }
}
