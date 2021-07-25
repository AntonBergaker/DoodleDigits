using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using Rationals;

namespace DoodleDigits.Core.Execution.Functions {
    public static class UnaryOperations {

        public static Value UnaryPlus(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is TooBigValue) {
                return value;
            }

            if (value is IConvertibleToReal convertibleToReal) {
                return convertibleToReal.ConvertToReal(context, context.Node.Value.Position);
            }

            if (value is RealValue real) {
                return real;
            }

            return new UndefinedValue();
        }

        public static Value UnaryNegate(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is TooBigValue tooBigValue) {
                return tooBigValue.Negate();
            }

            if (value is IConvertibleToReal convertibleToReal) {
                value = convertibleToReal.ConvertToReal(context, context.Node.Value.Position);
            }

            if (value is RealValue real) {
                return new RealValue(-real.Value);
            }

            return new UndefinedValue();
        }

        public static Value UnaryNot(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is IConvertibleToBool convertibleToBool) {
                value = convertibleToBool.ConvertToBool(context, context.Node.Value.Position);
            }

            if (value is BooleanValue @bool) {
                return new BooleanValue(!@bool.Value);
            }

            return new UndefinedValue();
        }

        private static Value IntegerFactorial(RealValue value) {
            Rational val = 1;
            if (value.Value > 10000) {
                return new TooBigValue(TooBigValue.Sign.Positive);
            }
            for (int i = 1; i <= value.Value; i++) {
                val *= i;
            }

            return new RealValue(val);
        }

        public static Value UnaryFactorial(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                value = convertibleToReal.ConvertToReal(context, context.Node.Value.Position);
            }

            if (value is RealValue real) {
                if (real.HasDecimal == false) {
                    return IntegerFactorial(real);
                }

                return Value.FromDouble(MathNet.Numerics.SpecialFunctions.Gamma((double)(1 + real.Value)));
            }

            return new UndefinedValue();
        }
    }
}
