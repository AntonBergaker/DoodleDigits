using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class UndefinedValue : Value {
        public override string ToString() {
            return "undefined";
        }
        
        public override bool Equals(Value? other) {
            return false;
        }

        public override int GetHashCode() {
            return 0;
        }
    }
}
