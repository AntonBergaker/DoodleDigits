using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary {

    public static partial class BinaryOperations {
        private static (RealValue lhs, RealValue rhs) ConvertToReal(IConvertibleToReal lhs, IConvertibleToReal rhs,
            ExecutionContext context, BinaryNodes nodes) {
            return (
                lhs.ConvertToReal(context, nodes.Lhs),
                rhs.ConvertToReal(context, nodes.Rhs)
            );
        }

        delegate Value? ImplementationFunction(Value other, BinaryOperation.OperationSide side,
            bool castAttempt, ExecutionContext context, BinaryNodes nodes);


        private static Value ExecuteBinaryImplementation(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes, ImplementationFunction lhsMethod, ImplementationFunction rhsMethod, Func<Value, Value, Value?>? fallback = null) {
            Value? result;
            
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, false, context, nodes);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, false, context, nodes);
            if (result != null) {
                return result;
            }

            // Try both sides again, but now allow casting
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, true, context, nodes);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, true, context, nodes);
            if (result != null) {
                return result;
            }

            if (fallback != null) {
                result = fallback(lhs, rhs);
                if (result != null) {
                    return result;
                }
            }

            // Ignore the operation if either side is undefined
            if (lhs is UndefinedValue && rhs is not UndefinedValue) {
                return rhs;
            }

            if (rhs is UndefinedValue) {
                return lhs;
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        private static Value ExecuteBinaryRealImplementation(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes, 
            ImplementationFunction lhsMethod, ImplementationFunction rhsMethod, Func<RealValue, ImplementationFunction> getRealFunc) =>
            ExecuteBinaryImplementation(lhs, rhs, context, nodes, lhsMethod, rhsMethod, (lhs, rhs) => {
                if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                    RealValue lhsReal = lhsCtr.ConvertToReal(context, nodes.Lhs);
                    RealValue rhsReal = rhsCtr.ConvertToReal(context, nodes.Rhs);

                    ImplementationFunction realFunc = getRealFunc(lhsReal);
                    return realFunc(rhsReal, BinaryOperation.OperationSide.Left, true, context, nodes);
                }
                return null;
            });


        public static Value Add(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TryAdd, rhs.TryAdd, x => x.TryAdd);

        public static Value Subtract(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TrySubtract, rhs.TrySubtract, x => x.TrySubtract);

        public static Value Divide(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TryDivide, rhs.TryDivide, x => x.TryDivide);

        public static Value Multiply(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TryMultiply, rhs.TryMultiply, x => x.TryMultiply);

        public static Value Modulus(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TryModulus, rhs.TryModulus, x => x.TryModulus);


        public static Value Power(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, nodes, lhs.TryPower, rhs.TryPower, x => x.TryPower);


        public static Value Cross(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            if (lhs is MatrixValue lhsMatrix && rhs is MatrixValue rhsMatrix) {
                return MatrixValue.Cross(lhsMatrix, rhsMatrix, context, nodes);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }
    }
}
