using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.Functions.Binary {

    public static partial class BinaryOperations {
        private static (BooleanValue lhs, BooleanValue rhs) ConvertToBool(IConvertibleToBool lhs, IConvertibleToBool rhs, ExecutionContext<BinaryOperation> context) {

            return (
                lhs.ConvertToBool(context.ForNode(context.Node.Lhs)),
                rhs.ConvertToBool(context.ForNode(context.Node.Rhs))
            );
        }

        public static Value BooleanAnd(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is not IConvertibleToBool ctbLhs || rhs is not IConvertibleToBool ctbRhs) {
                return new UndefinedValue();
            }

            var (boolLhs, boolRhs) = ConvertToBool(ctbLhs, ctbRhs, context);
            return new BooleanValue(boolLhs.Value && boolRhs.Value);
        }

        public static Value BooleanXor(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is not IConvertibleToBool ctbLhs || rhs is not IConvertibleToBool ctbRhs) {
                return new UndefinedValue();
            }

            var (boolLhs, boolRhs) = ConvertToBool(ctbLhs, ctbRhs, context);
            return new BooleanValue(boolLhs.Value ^ boolRhs.Value);
        }

        public static Value BooleanOr(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is not IConvertibleToBool ctbLhs || rhs is not IConvertibleToBool ctbRhs) {
                return new UndefinedValue();
            }

            var (boolLhs, boolRhs) = ConvertToBool(ctbLhs, ctbRhs, context);
            return new BooleanValue(boolLhs.Value || boolRhs.Value);
        }
    }


}
