using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;

namespace DoodleDigits.Core.Execution.Functions.Binary {

    public static class BoolBinaryOperations {
        private static (BooleanValue val0, BooleanValue val1) ConvertToBool(Value value0, Value value1, ExecutionContext context) {
            BinaryOperation bo = (BinaryOperation)context.Node;

            if (value0 is RealValue real0) {
                context.AddWarning("Conversion", bo.Right.Position);
                value0 = new BooleanValue(real0.Value > 0.5);
            }
            if (value1 is RealValue real1) {
                context.AddWarning("Conversion", bo.Left.Position);
                value1 = new BooleanValue(real1.Value > 0.5);
            }

            return ((BooleanValue)value0, (BooleanValue)value1);
        }
        
    }


}
