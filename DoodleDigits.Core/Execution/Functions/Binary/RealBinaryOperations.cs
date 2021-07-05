using System;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static partial class BinaryOperations {
        private static (RealValue val0, RealValue val1) ConvertToReal(Value value0, Value value1,
            ExecutionContext<BinaryOperation> context) {
            BinaryOperation bo = context.Node;


            if (value0 is BooleanValue bool0) {
                value0 = bool0.ConvertToReal();
                context.AddResult(new ResultConversion(bool0, value0, bo.Right.Position));
            }

            if (value1 is BooleanValue bool1) {
                value1 = bool1.ConvertToReal();
                context.AddResult(new ResultConversion(bool1, value1, bo.Left.Position));
            }

            return ((RealValue) value0, (RealValue) value1);
        }

        public static RealValue Add(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value + rValue1.Value);
        }

        public static RealValue Subtract(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value - rValue1.Value);
        }


        public static RealValue Divide(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value / rValue1.Value);
        }

        public static RealValue Multiply(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value * rValue1.Value);
        }


        public static RealValue Modulus(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(rValue0.Value % rValue1.Value);
        }


        public static RealValue Power(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (rValue0, rValue1) = ConvertToReal(value0, value1, context);
            return new(Math.Pow(rValue0.Value, rValue1.Value));
        }

        public static BooleanValue LessThan(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (realValue0, realValue1) = ConvertToReal(value0, value1, context);
            return new(realValue0.Value < realValue1.Value);
        }

        public static BooleanValue LessOrEqualTo(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (realValue0, realValue1) = ConvertToReal(value0, value1, context);
            return new(realValue0.Value <= realValue1.Value);
        }

        public static BooleanValue GreaterThan(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (realValue0, realValue1) = ConvertToReal(value0, value1, context);
            return new(realValue0.Value > realValue1.Value);
        }

        public static BooleanValue GreaterOrEqualTo(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (realValue0, realValue1) = ConvertToReal(value0, value1, context);
            return new(realValue0.Value >= realValue1.Value);
        }
    }
}
