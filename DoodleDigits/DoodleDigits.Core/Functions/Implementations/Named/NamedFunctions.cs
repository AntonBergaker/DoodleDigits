﻿using System;
using System.Linq;
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            var value = ConvertArgumentToReal(convertibleToReal0, 0, context);

            if (value.Value <= Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            if (values.Length == 1) {
                return Value.FromDouble(Rational.Log10( value.Value ), false, value.Form);
            }

            if (values[1] is TooBigValue {IsPositive: true}) {
                return new RealValue(Rational.Zero, false, value.Form);
            }

            if (values[1] is not IConvertibleToReal convertibleToReal1) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
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
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                if (dValue.Value < Rational.Zero && dRoot.Value.Modulus(2) == 0) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return Value.FromDouble(Math.Pow((double) dValue.Value, (double) (1 / dRoot.Value)), false, dValue.Form);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        [CalculatorFunction("ln")]
        public static Value Ln(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                if (realValue.Value <= Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return Value.FromDouble(Rational.Log(realValue.Value), false, realValue.Form);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        [CalculatorFunction(2, int.MaxValue, "gcd", "gcf")]
        public static Value GreatestCommonDivisor(Value[] values, ExecutionContext<Function> context) {
            int startIndex = 0;
            RealValue? realValue = null;

            // Find the first convertible value
            while (startIndex < values.Length) {
                if (values[startIndex] is IConvertibleToReal valueCtr) {
                    var childContext = context.ForNode(context.Node.Arguments[startIndex]);
                    realValue = valueCtr.ConvertToReal(context).Round(childContext);
                    break;
                }

                startIndex++;
            }

            if (realValue == null) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            for (int i = startIndex+1; i < values.Length; i++) {
                if (values[i] is IConvertibleToReal valueCtr) {
                    var childContext = context.ForNode(context.Node.Arguments[startIndex]);
                    var newRealValue = valueCtr.ConvertToReal(childContext).Round(childContext);

                    realValue = new RealValue(BigInteger.GreatestCommonDivisor(realValue.Value.Numerator, newRealValue.Value.Numerator), false,
                        realValue.Form);
                }

            }

            return realValue;
        }

        [CalculatorFunction("sqrt", "square_root")]
        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                if (realValue.Value < Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return BinaryOperations.Power(realValue, new RealValue(RationalUtils.Half));
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        [CalculatorFunction("abs", "absolute")]
        public static Value Abs(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? tbv : tbv.Negate();
            }

            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                return new RealValue(Rational.Abs(realValue.Value), false, realValue.Form);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        [CalculatorFunction("sign", "sig")]
        public static Value Sign(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? new RealValue(Rational.One) : new RealValue(-1);
            }

            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                return new RealValue(realValue.Value.Sign, false, realValue.Form);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }
            return new RealValue(min.Value, false, form);
        }

        [CalculatorFunction("floor")]
        public static Value Floor(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
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
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form);
        }
    }
}
