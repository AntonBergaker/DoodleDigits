using System;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static class RealBinaryOperations {
        private static (RealValue val0, RealValue val1) ConvertToReal(Value value0, Value value1,
            ExecutionContext context) {
            BinaryOperation bo = (BinaryOperation) context.Node;


            if (value0 is BooleanValue bool0) {
                context.AddWarning("Conversion", bo.Right.Position);
                value0 = new RealValue(bool0.Value ? 0 : 1);
            }

            if (value1 is BooleanValue bool1) {
                context.AddWarning("Conversion", bo.Left.Position);
                value1 = new RealValue(bool1.Value ? 0 : 1);
            }

            return ((RealValue) value0, (RealValue) value1);
        }

        public static RealValue Add(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value + rValue1.Value);
        }

        public static RealValue Subtract(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value - rValue1.Value);
        }


        public static RealValue Divide(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value / rValue1.Value);
        }

        public static RealValue Multiply(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value * rValue1.Value);
        }


        public static RealValue Modulus(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value % rValue1.Value);
        }


        public static RealValue Power(Value value0, Value value1, ExecutionContext context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(Math.Pow(rValue0.Value, rValue1.Value));
        }
    }
}
