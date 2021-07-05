using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.Functions {
    public static class UnaryOperations {

        public static Value UnaryPlus(Value value, ExecutionContext<UnaryOperation> context) {

            if (value is BooleanValue @bool) {
                value = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, value, context.Node.Position));
            }

            if (value is RealValue real) {
                return real;
            }

            throw new NotImplementedException();
        }

        public static Value UnaryNegate(Value value, ExecutionContext<UnaryOperation> context) {

            if (value is BooleanValue @bool) {
                value = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, value, context.Node.Position));
            }

            if (value is RealValue real) {
                return new RealValue(-real.Value);
            }

            throw new NotImplementedException();
        }

    }
}
