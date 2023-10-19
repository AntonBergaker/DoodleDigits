using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution {

    public class ExecutionContext {

        private readonly Dictionary<string, Value> constants;
        private readonly Dictionary<string, Value> variables;
        private readonly List<Result> results;

        public IReadOnlyDictionary<string, Value> Constants => constants;
        public Dictionary<string, Value> Variables => variables;
        public IReadOnlyList<Result> Results => results;

        public ExecutionContext(IEnumerable<Constant> constants) {
            this.constants = constants.ToDictionary(x => x.Name, x => x.Value);
            this.variables = new();
            this.results = new();
        }

        public void AddResult(Result result) {
            results.Add(result);
        }

        public void Clear() {
            results.Clear();
            variables.Clear();
        }
    }
}
