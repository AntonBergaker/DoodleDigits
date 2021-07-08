using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.ValueTypes {
    class UndefinedValue : Value {
        public override string ToString() {
            return "undefined";
        }

        public override bool IsAbstract => true;
    }
}
