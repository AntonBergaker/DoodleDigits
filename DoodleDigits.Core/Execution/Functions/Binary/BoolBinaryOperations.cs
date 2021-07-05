using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.Functions.Binary {

    public static partial class BinaryOperations {
        private static (BooleanValue val0, BooleanValue val1) ConvertToBool(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            BinaryOperation bo = context.Node;

            if (value0 is RealValue real0) {
                value0 = new BooleanValue(real0.Value > 0.5);
                context.AddResult(new ResultConversion(real0, value0, bo.Right.Position));
            }
            if (value1 is RealValue real1) {
                value1 = new BooleanValue(real1.Value > 0.5);
                context.AddResult(new ResultConversion(real1, value1, bo.Right.Position));
            }

            return ((BooleanValue)value0, (BooleanValue)value1);
        }

        public static BooleanValue And(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (boolValue0, boolValue1) = ConvertToBool(value0, value1, context);
            return new(boolValue0.Value && boolValue1.Value);
        }

        public static BooleanValue Xor(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (boolValue0, boolValue1) = ConvertToBool(value0, value1, context);
            return new(boolValue0.Value ^ boolValue1.Value);
        }

        public static BooleanValue Or(Value value0, Value value1, ExecutionContext<BinaryOperation> context) {
            var (boolValue0, boolValue1) = ConvertToBool(value0, value1, context);
            return new(boolValue0.Value || boolValue1.Value);
        }
    }


}
