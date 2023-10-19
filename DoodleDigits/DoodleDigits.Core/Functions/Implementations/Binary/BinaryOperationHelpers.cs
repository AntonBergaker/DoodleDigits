using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Functions.Implementations.Binary {


    public static class BinaryOperationHelpers {
        /// <summary>
        /// Returns value if it's a realvalue. Tries to convert it if shouldConvert is true. Utility function since this is a common if statement
        /// </summary>
        public static bool TryConvertToReal(Value value, bool shouldConvert, BinaryOperation.OperationSide side, ExecutionContext context, BinaryNodes nodes, [NotNullWhen(true)] out RealValue? realValue) {
            if (value is RealValue rv) {
                realValue = rv;
                return true;
            }

            if (shouldConvert && value is IConvertibleToReal ctr) {
                realValue = ctr.ConvertToReal(context, nodes, side);
                return true;
            }

            realValue = null;
            return false;
        }
        
        public static RealValue ConvertToReal(this IConvertibleToReal convertible, ExecutionContext context, BinaryNodes nodes, BinaryOperation.OperationSide side) {
            if (side == BinaryOperation.OperationSide.Left) {
                return convertible.ConvertToReal(context, nodes.Lhs);
            }
            if (side == BinaryOperation.OperationSide.Right) {
                return convertible.ConvertToReal(context, nodes.Rhs);
            }

            throw new InvalidEnumArgumentException();
        }

        /// <summary>
        /// In the context binary operations are used, if other is rhs or lhs depends on side. This will make the side consistent
        /// </summary>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public static (T rhs, T lhs) GetLhsRhs<T>(T @this, T other, BinaryOperation.OperationSide side) where T : Value {
            if (side == BinaryOperation.OperationSide.Left) {
                return (@this, other);
            }
            else {
                return (other, @this);
            }
        }
    }
}
