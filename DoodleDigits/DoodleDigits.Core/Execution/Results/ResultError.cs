using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.Results {
    public class ResultError : Result {
        public string Error { get; }

        public ResultError(string error, Range position) : base(position) {
            Error = error;
        }
    }
}
