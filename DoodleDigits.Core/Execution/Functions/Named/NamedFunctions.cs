using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using Rationals;

namespace DoodleDigits.Core.Execution {
    public static class NamedFunctions {

        private static RealValue ConvertArgumentToReal(IConvertibleToReal[] values, int index, ExecutionContext<Function> context) {
            return ConvertArgumentToReal(values[index], index, context);
        }

        private static RealValue ConvertArgumentToReal(IConvertibleToReal value, int index, ExecutionContext<Function> context) {
            Function func = context.Node;

            return value.ConvertToReal(context, context.Node.Arguments[index].Position);
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

        public static Value Sin(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            return Value.FromDouble(Math.Sin((double)ConvertArgumentToReal(convertibleToReal, 0, context).Value));
        }

        public static Value Cos(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }
            return Value.FromDouble(Math.Cos((double) ConvertArgumentToReal(convertibleToReal, 0, context).Value));
        }

        public static Value Tan(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }
            return Value.FromDouble(Math.Sin((double) ConvertArgumentToReal(convertibleToReal, 0, context).Value));
        }

        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            if (value is TooBigValue {IsPositive: false}) {
                return new UndefinedValue();
            }

            if (value is TooBigValue) {
                return value;
            } 

            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
            if (realValue.Value < 0) {
                return new UndefinedValue();
            }
            return Value.FromDouble( Math.Sqrt((double) realValue.Value));
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
    }
}
