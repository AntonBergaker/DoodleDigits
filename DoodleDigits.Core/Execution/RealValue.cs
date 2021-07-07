using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution {
    public class RealValue : Value {
        public readonly double Value;

        public RealValue(double value) {
            Value = value;
        }

        public override string ToString() {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public Value ConvertToBool() {
            return new BooleanValue(Value > 0.5);
        }

        public bool HasDecimal => Value % 1 == 0;
    }
}
