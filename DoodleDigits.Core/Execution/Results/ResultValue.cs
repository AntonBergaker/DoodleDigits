using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution.Results {
    public class ResultValue : Result {
        public Value Value { get; }

        public ResultValue(Value value, Range position) : base(position) {
            Value = value;
        }
    }
}
