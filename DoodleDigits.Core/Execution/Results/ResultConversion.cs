using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.Results {
    class ResultConversion : Result {
        public Value PreviousValue { get; }
        public Value NewValue { get; }

        public ResultConversion(Value previousValue, Value newValue, Range position) : base(position) {
            PreviousValue = previousValue;
            NewValue = newValue;
        }
    }
}
