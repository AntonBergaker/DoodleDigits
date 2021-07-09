using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution.Results {
    public class ResultConversion : Result {
        public Value PreviousValue { get; }
        public Value NewValue { get; }

        public ResultConversion(Value previousValue, Value newValue, Range position) : base(position) {
            PreviousValue = previousValue;
            NewValue = newValue;
        }
    }
}
