using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.Results {
    public abstract class Result {
        public Range Position { get; }

        protected Result(Range position) {
            Position = position;
        }

    }
}
