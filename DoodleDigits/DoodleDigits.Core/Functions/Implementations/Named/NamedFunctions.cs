using System;
using System.Numerics;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using MathNet.Numerics;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Named {
    public static partial class NamedFunctions {

        private static RealValue ConvertArgumentToReal(IConvertibleToReal value, int index, ExecutionContext<Function> context) {
            return value.ConvertToReal(context.ForNode(context.Node.Arguments[index]));
        }
        
        [CalculatorFunction(1, 2, "log")]
        public static Value Log(Value[] values, ExecutionContext<Function> context) {
            if (values[0] is not IConvertibleToReal convertibleToReal0) {
                return new UndefinedValue();
            }

            var value = ConvertArgumentToReal(convertibleToReal0, 0, context);

            if (values.Length == 1) {
                return Value.FromDouble(Rational.Log10( value.Value ), false, value.Form);
            }

            if (values[1] is TooBigValue {IsPositive: true}) {
                return new RealValue(0, false, value.Form);
            }

            if (values[1] is not IConvertibleToReal convertibleToReal1) {
                return new UndefinedValue();
            }

            var @base = ConvertArgumentToReal(convertibleToReal1, 1, context);

            return Value.FromDouble(Rational.Log(
                value.Value,
                (double)@base.Value
            ), false, value.Form);
        }

        [CalculatorFunction("root")]
        public static Value Root(Value value, Value root, ExecutionContext<Function> context) {

            if (value is IConvertibleToReal rValue && root is IConvertibleToReal rRoot) {
                var dValue = ConvertArgumentToReal(rValue, 0, context);
                var dRoot = ConvertArgumentToReal(rRoot, 1, context);

                if (dRoot.Value == Rational.Zero) {
                    return new UndefinedValue();
                }

                return Value.FromDouble(Math.Pow((double) dValue.Value, (double) (1 / dRoot.Value)), false, dValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("ln")]
        public static Value Ln(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Rational.Log(realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("gcd", "gcf")]
        public static Value GreatestCommonDivisor(Value value0, Value value1, ExecutionContext<Function> context) {
            if (value0 is not IConvertibleToReal value0Ctr || value1 is not IConvertibleToReal value1Ctr) {
                return new UndefinedValue();
            }

            var value0Real = ConvertArgumentToReal(value0Ctr, 0, context);
            var value1Real = ConvertArgumentToReal(value1Ctr, 0, context);
            value0Real = value0Real.Round(context, context.Node.Arguments[0].Position);
            value1Real = value1Real.Round(context, context.Node.Arguments[1].Position);

            return new RealValue(BigInteger.GreatestCommonDivisor(value0Real.Value.Numerator, value1Real.Value.Numerator), false, value0Real.Form);
        }

        [CalculatorFunction("sqrt", "square_root")]
        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            return BinaryOperations.Power(ConvertArgumentToReal(convertibleToReal, 0, context), new RealValue(RationalUtils.Half));
        }

        [CalculatorFunction("abs", "absolute")]
        public static Value Abs(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? tbv : tbv.Negate();
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return new RealValue(Rational.Abs(realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("sign", "sig")]
        public static Value Sign(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? new RealValue(Rational.One) : new RealValue(-1);
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return new RealValue(realValue.Value.Sign, false, realValue.Form);
        }

        [CalculatorFunction(1, int.MaxValue,  "max")]
        public static Value Max(Value[] values, ExecutionContext<Function> context) {
            Rational? max = null;
            RealValue.PresentedForm form = RealValue.PresentedForm.Unset;

            for (var index = 0; index < values.Length; index++) {
                Value value = values[index];
                if (value is TooBigValue {IsPositive: true}) {
                    return value;
                }

                if (value is not IConvertibleToReal convertibleToReal) {
                    continue;
                }

                RealValue realValue = ConvertArgumentToReal(convertibleToReal, index, context);
                if (max == null) {
                    form = realValue.Form;
                    max = realValue.Value;
                    continue;
                }
                if (realValue.Value > max) {
                    max = realValue.Value;
                }
            }

            if (max == null) {
                return new UndefinedValue();
            }
            return new RealValue(max.Value, false, form);
        }

        [CalculatorFunction(1, int.MaxValue, "min")]
        public static Value Min(Value[] values, ExecutionContext<Function> context) {
            Rational? min = null;
            RealValue.PresentedForm form = RealValue.PresentedForm.Unset;

            for (var index = 0; index < values.Length; index++) {
                Value value = values[index];
                if (value is TooBigValue { IsPositive: false }) {
                    return value;
                }

                if (value is not IConvertibleToReal convertibleToReal) {
                    continue;
                }

                RealValue realValue = ConvertArgumentToReal(convertibleToReal, index, context);
                if (min == null) {
                    min = realValue.Value;
                    form = realValue.Form;
                    continue;
                }

                if (realValue.Value < min) {
                    min = realValue.Value;
                }
            
            }

            if (min == null) {
                return new UndefinedValue();
            }
            return new RealValue(min.Value, false, form);
        }

        [CalculatorFunction("floor")]
        public static Value Floor(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Floor(realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("round")]
        public static Value Round(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("ceil", "ceiling")]
        public static Value Ceil(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form);
        }
    }
}
