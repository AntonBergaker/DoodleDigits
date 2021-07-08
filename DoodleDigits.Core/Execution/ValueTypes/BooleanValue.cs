using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution {
    public class BooleanValue : Value, IConvertibleToReal, IConvertibleToBool {
        public readonly bool Value;

        public BooleanValue(bool value) {
            Value = value;
        }

        public override string ToString() {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override bool IsAbstract => false;

        public RealValue ConvertToReal() {
            return new RealValue(Value ? 1 : 0);
        }

        public RealValue ConvertToReal(ExecutionContext context, Range position) {
            RealValue newValue = ConvertToReal();
            context.AddResult(new ResultConversion(this, newValue, position));
            return newValue;
        }

        public BooleanValue ConvertToBool() {
            return this;
        }

        public BooleanValue ConvertToBool(ExecutionContext context, Range position) {
            return this;
        }
    }
}
