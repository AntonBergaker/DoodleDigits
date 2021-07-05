using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution {

    public class ExecutionContext {
        protected class ExecutionContextData {
            public readonly Dictionary<string, Value> Constants;
            public readonly Dictionary<string, Value> Variables;
            public readonly List<Result> Results;

            public ExecutionContextData(IEnumerable<Constant> constants) {
                Constants = constants.ToDictionary(x => x.Name, x => x.Value);
                Variables = new();
                Results = new List<Result>();
            }
        }

        protected ExecutionContextData data;

        public IReadOnlyDictionary<string, Value> Constants => data.Constants;
        public Dictionary<string, Value> Variables => data.Variables;

        public ExecutionContext(IEnumerable<Constant> constants) {
            data = new ExecutionContextData(constants);
        }

        protected ExecutionContext(ExecutionContext other) {
            this.data = other.data;
        }

        public void AddResult(Result result) {
            data.Results.Add(result);
        }

        public ExecutionContext<T> ForNode<T>(T node) where T: AstNode {
            return new ExecutionContext<T>(this, node);
        }

    }


    public class ExecutionContext<T> : ExecutionContext where T: AstNode {

        public ExecutionContext(ExecutionContext context, T node) : base(context) {
            Node = node;
        }

        public T Node { private set; get; }

    }
}
