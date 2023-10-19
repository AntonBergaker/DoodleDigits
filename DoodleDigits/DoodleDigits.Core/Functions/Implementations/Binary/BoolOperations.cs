using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Functions.Implementations.Binary {

    public static partial class BinaryOperations {
        private static (BooleanValue lhs, BooleanValue rhs) ConvertToBool(IConvertibleToBool lhs, IConvertibleToBool rhs, ExecutionContext context, BinaryNodes nodes) {

            return (
                lhs.ConvertToBool(context, nodes.Lhs),
                rhs.ConvertToBool(context, nodes.Rhs)
            );
        }

        private static Value BooleanOperation(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes, Func<bool, bool, bool> func) {
            if (lhs is IConvertibleToBool ctbLhs && rhs is IConvertibleToBool ctbRhs) {
                var (boolLhs, boolRhs) = ConvertToBool(ctbLhs, ctbRhs, context, nodes);
                return new BooleanValue(func(boolLhs.Value, boolRhs.Value));
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value BooleanAnd(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return BooleanOperation(lhs, rhs, context, nodes, (lhs, rhs) =>  lhs && rhs);
        }

        public static Value BooleanXor(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return BooleanOperation(lhs, rhs, context, nodes, (lhs, rhs) => lhs ^ rhs);
        }

        public static Value BooleanOr(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return BooleanOperation(lhs, rhs, context, nodes, (lhs, rhs) => lhs || rhs);
        }
    }


}
