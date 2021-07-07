using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.Functions {
    public static class UnaryOperations {

        public static Value UnaryPlus(Value value, ExecutionContext<UnaryOperation> context) {

            if (value is BooleanValue @bool) {
                value = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, value, context.Node.Value.Position));
            }

            if (value is RealValue real) {
                return real;
            }

            throw new NotImplementedException();
        }

        public static Value UnaryNegate(Value value, ExecutionContext<UnaryOperation> context) {

            if (value is BooleanValue @bool) {
                value = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, value, context.Node.Value.Position));
            }

            if (value is RealValue real) {
                return new RealValue(-real.Value);
            }

            throw new NotImplementedException();
        }

        public static Value UnaryNot(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is RealValue real) {
                value = real.ConvertToBool();
                context.AddResult(new ResultConversion(real, value, context.Node.Value.Position));
            }

            if (value is BooleanValue @bool) {
                return new BooleanValue(!@bool.Value);
            }

            throw new NotImplementedException();
        }

        private static Value IntegerFactorial(RealValue value) {
            double val = 1;
            if (val > 1000) {
                return new RealValue(double.PositiveInfinity);
            }
            for (int i = 1; i <= value.Value; i++) {
                val *= i;
            }

            return new RealValue(val);
        }

        public static Value UnaryFactorial(Value value, ExecutionContext<UnaryOperation> context) {
            if (value is BooleanValue @bool) {
                value = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, value, context.Node.Value.Position));
            }

            if (value is RealValue real) {
                if (real.HasDecimal) {
                    return IntegerFactorial(real);
                }
                return new RealValue(MathNet.Numerics.SpecialFunctions.Gamma(1 + real.Value));
            }

            throw new NotImplementedException();
        }
    }
}
