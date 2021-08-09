using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Functions.Binary;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution {
    public static partial class NamedFunctions {

        private static RealValue ConvertArgumentToReal(IConvertibleToReal value, int index, ExecutionContext<Function> context) {
            return value.ConvertToReal(context.ForNode(context.Node.Arguments[index]));
        }
        

        public static Value Log(Value[] values, ExecutionContext<Function> context) {
            if (values[0] is not IConvertibleToReal convertibleToReal0) {
                return new UndefinedValue();
            }

            var value = ConvertArgumentToReal(convertibleToReal0, 0, context);

            if (values.Length == 1) {
                return Value.FromDouble(Rational.Log10( value.Value ));
            }

            if (values[1] is TooBigValue {IsPositive: true}) {
                return new RealValue(0);
            }

            if (values[1] is not IConvertibleToReal convertibleToReal1) {
                return new UndefinedValue();
            }

            var @base = ConvertArgumentToReal(convertibleToReal1, 1, context);

            return Value.FromDouble(Rational.Log(
                value.Value,
                (double)@base.Value
            ));
        }

        public static Value Root(Value value, Value root, ExecutionContext<Function> context) {

            if (value is IConvertibleToReal rValue && root is IConvertibleToReal rRoot) {
                var dValue = ConvertArgumentToReal(rValue, 0, context).Value;
                var dRoot = ConvertArgumentToReal(rRoot, 1, context).Value;

                if (dRoot == Rational.Zero) {
                    return new UndefinedValue();
                }

                return Value.FromDouble(Math.Pow((double) dValue, (double) (1 / dRoot)));
            }

            return new UndefinedValue();
        }

        public static Value Ln(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            return Value.FromDouble(Rational.Log(ConvertArgumentToReal(convertibleToReal, 0, context).Value));
        }

        public static Value GreatestCommonDivisor(Value value0, Value value1, ExecutionContext<Function> context) {
            if (value0 is not IConvertibleToReal value0Ctr || value1 is not IConvertibleToReal value1Ctr) {
                return new UndefinedValue();
            }

            var value0Real = ConvertArgumentToReal(value0Ctr, 0, context);
            var value1Real = ConvertArgumentToReal(value1Ctr, 0, context);
            value0Real = value0Real.Round(context, context.Node.Arguments[0].Position);
            value1Real = value1Real.Round(context, context.Node.Arguments[1].Position);

            return new RealValue(BigInteger.GreatestCommonDivisor(value0Real.Value.Numerator, value1Real.Value.Numerator));
        }

        
        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            return BinaryOperations.Power(ConvertArgumentToReal(convertibleToReal, 0, context), new RealValue(RationalUtils.Half));
        }

        public static Value Abs(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? tbv : tbv.Negate();
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return new RealValue(Rational.Abs(realValue.Value));
        }

        public static Value Sign(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue tbv) {
                return tbv.IsPositive ? new RealValue(Rational.One) : new RealValue(-1);
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return new RealValue(realValue.Value.Sign);
        }

        public static Value Max(Value[] values, ExecutionContext<Function> context) {
            Rational? max = null;
            
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
            return new RealValue(max.Value);
        }

        public static Value Min(Value[] values, ExecutionContext<Function> context) {
            Rational? min = null;

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
                    continue;
                }

                if (realValue.Value < min) {
                    min = realValue.Value;
                }
            
            }

            if (min == null) {
                return new UndefinedValue();
            }
            return new RealValue(min.Value);
        }


        public static Value Floor(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Floor(realValue.Value));
        }

        public static Value Round(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value));
        }

        public static Value Ceil(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            return new RealValue(RationalUtils.Round(realValue.Value));
        }
    }
}
