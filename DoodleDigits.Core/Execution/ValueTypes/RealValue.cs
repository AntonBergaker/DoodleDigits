using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using Rationals;

namespace DoodleDigits.Core.Execution {
    public class RealValue : Value, IConvertibleToReal, IConvertibleToBool {
        public readonly Rational Value;

        public RealValue(Rational value) {
            Value = value;
        }

        public override string ToString() {
            return Value.ToString();
        }

        public override bool IsAbstract => false;

        public BooleanValue ConvertToBool() {
            return new BooleanValue(Value > new Rational(1, 2));
        }

        public BooleanValue ConvertToBool(ExecutionContext context, Range position) {
            BooleanValue newValue = ConvertToBool();
            context.AddResult(new ResultConversion(this, newValue, position));
            return newValue;
        }

        public bool HasDecimal => Value.FractionPart != 0;
        public RealValue ConvertToReal() {
            return this;
        }

        public RealValue ConvertToReal(ExecutionContext context, Range position) {
            return this;
        }
    }
}
