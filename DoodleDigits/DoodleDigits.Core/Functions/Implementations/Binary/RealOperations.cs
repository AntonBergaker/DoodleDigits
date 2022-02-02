using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary {

    public static partial class BinaryOperations {
        private static (RealValue lhs, RealValue rhs) ConvertToReal(IConvertibleToReal lhs, IConvertibleToReal rhs,
            ExecutionContext<BinaryOperation> context) {
            return (
                lhs.ConvertToReal(context.ForNode(context.Node.Lhs)),
                rhs.ConvertToReal(context.ForNode(context.Node.Rhs))
            );
        }

        delegate Value? ImplementationFunction(Value other, BinaryOperation.OperationSide side,
            bool castAttempt, ExecutionContext<BinaryOperation> context);


        private static Value ExecuteBinaryImplementation(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context, ImplementationFunction lhsMethod, ImplementationFunction rhsMethod, Func<Value, Value, Value?>? fallback = null) {
            Value? result;
            
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, false, context);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, false, context);
            if (result != null) {
                return result;
            }

            // Try both sides again, but now allow casting
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, true, context);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, true, context);
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

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        private static Value ExecuteBinaryRealImplementation(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context, 
            ImplementationFunction lhsMethod, ImplementationFunction rhsMethod, Func<RealValue, ImplementationFunction> getRealFunc) =>
            ExecuteBinaryImplementation(lhs, rhs, context, lhsMethod, rhsMethod, (lhs, rhs) => {
                if (lhs is IConvertibleToReal lhsCtr && rhs is IConvertibleToReal rhsCtr) {
                    RealValue lhsReal = lhsCtr.ConvertToReal(context.ForNode(context.Node.Lhs));
                    RealValue rhsReal = rhsCtr.ConvertToReal(context.ForNode(context.Node.Rhs));

                    ImplementationFunction realFunc = getRealFunc(lhsReal);
                    return realFunc(rhsReal, BinaryOperation.OperationSide.Left, true, context);
                }
                return null;
            });


        public static Value Add(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TryAdd, rhs.TryAdd, x => x.TryAdd);

        public static Value Subtract(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TrySubtract, rhs.TrySubtract, x => x.TrySubtract);

        public static Value Divide(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TryDivide, rhs.TryDivide, x => x.TryDivide);

        public static Value Multiply(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TryMultiply, rhs.TryMultiply, x => x.TryMultiply);

        public static Value Modulus(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TryModulus, rhs.TryModulus, x => x.TryModulus);


        public static Value Power(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryRealImplementation(lhs, rhs, context, lhs.TryPower, rhs.TryPower, x => x.TryPower);


        public static Value Cross(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is MatrixValue lhsMatrix && rhs is MatrixValue rhsMatrix) {
                return MatrixValue.Cross(lhsMatrix, rhsMatrix, context);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }
    }
}
