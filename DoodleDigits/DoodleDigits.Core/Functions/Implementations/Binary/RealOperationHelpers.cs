using System.ComponentModel;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Functions.Implementations.Binary {
    public static class RealOperationHelpers {
        public static RealValue ConvertToReal(this IConvertibleToReal convertible, ExecutionContext<BinaryOperation> context, BinaryOperation.OperationSide side) {
            if (side == BinaryOperation.OperationSide.Left) {
                return convertible.ConvertToReal(context.ForNode(context.Node.Lhs));
            }
            if (side == BinaryOperation.OperationSide.Right) {
                return convertible.ConvertToReal(context.ForNode(context.Node.Rhs));
            }

            throw new InvalidEnumArgumentException();
        }
    }
}
