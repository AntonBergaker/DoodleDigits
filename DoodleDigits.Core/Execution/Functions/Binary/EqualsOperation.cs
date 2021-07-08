using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

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

        public static Value Equals(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            {
                if (lhs is BooleanValue bLhs && rhs is BooleanValue bRhs) {
                    return new BooleanValue(bLhs.Value == bRhs.Value);
                }
            }

            {
                if (lhs is TooBigValue tooBigLhs && rhs is TooBigValue tooBigRhs) {
                    return new BooleanValue(tooBigLhs.ValueSign == tooBigRhs.ValueSign);
                }

                if (lhs is TooBigValue || rhs is TooBigValue) {
                    return new BooleanValue(false);
                }
            }

            {
                if (lhs is UndefinedValue || rhs is UndefinedValue) {
                    return lhs;
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

        public static Value NotEquals(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            Value equalsValue = Equals(value0, value1, context);
            if (equalsValue is BooleanValue @bool) {
                return new BooleanValue(!@bool.Value);
            }
            
            return equalsValue;
        }
    }
}
