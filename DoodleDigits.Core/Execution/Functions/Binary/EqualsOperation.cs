using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static partial class BinaryOperations {

        public static Value Equals(Identifier identifier, Value assignment, ExecutionContext<BinaryOperation> context) {
            if (context.Variables.TryGetValue(identifier.Value, out Value? variableValue)) {
                return Equals(variableValue, assignment, context);
            }

            if (context.Constants.TryGetValue(identifier.Value, out Value? constantValue)) {
                return Equals(constantValue, assignment, context);
            }

            context.Variables[identifier.Value] = assignment;
            return assignment;
        }

        public static BooleanValue Equals(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            {
                if (lhs is BooleanValue bLhs && rhs is BooleanValue bRhs) {
                    return new BooleanValue(bLhs.Value == bRhs.Value);
                }
            }

            {
                RealValue? realLhs = lhs as RealValue;
                RealValue? realRhs = rhs as RealValue;

                if (lhs is BooleanValue boolLhs) {
                    realLhs = boolLhs.ConvertToReal();
                    context.AddResult(new ResultConversion(boolLhs, realLhs, context.Node.Left.Position));
                }
                if (rhs is BooleanValue boolRhs) {
                    realRhs = boolRhs.ConvertToReal();
                    context.AddResult(new ResultConversion(boolRhs, realRhs, context.Node.Right.Position));
                }

                if (realLhs != null && realRhs != null) {
                    return new BooleanValue(realLhs.Value.Equals(realRhs.Value));
                }

                throw new NotImplementedException();
            }
        }

        public static BooleanValue NotEquals(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            return new BooleanValue(!Equals(value0, value1, context).Value);
        }
    }
}
