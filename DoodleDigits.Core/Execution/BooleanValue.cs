using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution {
    public class BooleanValue : Value {
        public readonly bool Value;

        public BooleanValue(bool value) {
            Value = value;
        }

        public override string ToString() {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
