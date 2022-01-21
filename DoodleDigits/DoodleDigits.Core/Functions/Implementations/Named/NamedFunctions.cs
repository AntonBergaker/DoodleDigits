using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Named {
    public static partial class NamedFunctions {

        private static RealValue ConvertArgumentToReal(IConvertibleToReal value, int index, ExecutionContext<Function> context) {
            return value.ConvertToReal(context.ForNode(context.Node.Arguments[index]));
        }
        
        [CalculatorFunction(FunctionExpectedType.Real, 1, 2, "log")]
        public static Value Log(Value[] values, ExecutionContext<Function> context) {
            if (values[0] is not IConvertibleToReal convertibleToReal0) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            var value = ConvertArgumentToReal(convertibleToReal0, 0, context);

            if (value.Value <= Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined, context.Node);
            }

            if (values.Length == 1) {
                return RealValue.FromDouble(Rational.Log10( value.Value ), false, value.Form, context.Node);
            }

            if (values[1] is TooBigValue {IsPositive: true}) {
                return new RealValue(Rational.Zero, false, value.Form, context.Node);
            }

            if (values[1] is not IConvertibleToReal convertibleToReal1) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            var @base = ConvertArgumentToReal(convertibleToReal1, 1, context);

            return RealValue.FromDouble(Rational.Log(
                value.Value,
                (double)@base.Value
            ), false, value.Form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "root")]
        public static Value Root(Value value, Value root, ExecutionContext<Function> context) {

            if (value is IConvertibleToReal rValue && root is IConvertibleToReal rRoot) {
                var dValue = ConvertArgumentToReal(rValue, 0, context);
                var dRoot = ConvertArgumentToReal(rRoot, 1, context);

                if (dRoot.Value == Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined, context.Node);
                }

                if (dValue.Value < Rational.Zero && dRoot.Value.Modulus(2) == 0) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined, context.Node);
                }

                return RealValue.FromDouble(Math.Pow((double) dValue.Value, (double) (1 / dRoot.Value)), false, dValue.Form, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "ln")]
        public static Value Ln(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                if (realValue.Value <= Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined, context.Node);
                }

                return RealValue.FromDouble(Rational.Log(realValue.Value), false, realValue.Form, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, 2, int.MaxValue, "gcd", "gcf")]
        public static Value GreatestCommonDivisor(Value[] values, ExecutionContext<Function> context) {
            List<RealValue> realValues = new();
            for (int i = 0; i < values.Length; i++) {
                if (values[i] is not IConvertibleToReal valueCtr) {
                    continue;
                }

                var childContext = context.ForNode(context.Node.Arguments[i]);
                realValues.Add(valueCtr.ConvertToReal(context).Round(childContext));
            }

            if (realValues.Count == 0) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            RealValue firstValue = realValues.First();
            BigInteger value = firstValue.Value.Numerator;

            for (int i = 1; i < realValues.Count; i++) {
                RealValue realValue = realValues[i];
                value = BigInteger.GreatestCommonDivisor(value, realValue.Value.Numerator);
            }

            return new RealValue(value, false, firstValue.Form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "sqrt", "square_root")]
        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                if (realValue.Value < Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined, context.Node);
                }

                return new RealValue(RationalUtils.Sqrt(realValue.Value), false, realValue.Form, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real | FunctionExpectedType.Vector, "abs", "absolute")]
        public static Value Abs(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? tbv : tbv.Negate();
            }

            if (value is MatrixValue) {
                return VectorFunctions.Magnitude(value, context);
            }

            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                return new RealValue(Rational.Abs(realValue.Value), false, realValue.Form, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "sign", "sig")]
        public static Value Sign(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? new RealValue(Rational.One) : new RealValue(-1);
            }

            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                return new RealValue(realValue.Value.Sign, false, realValue.Form, context.Node);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue,  "max")]
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }
            return new RealValue(max.Value, false, form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "min")]
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }
            return new RealValue(min.Value, false, form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "floor")]
        public static Value Floor(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Floor(realValue.Value), false, realValue.Form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "round")]
        public static Value Round(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Real, "ceil", "ceiling")]
        public static Value Ceil(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form, context.Node);
        }
    }
}
