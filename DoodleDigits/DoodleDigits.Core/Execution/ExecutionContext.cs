using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution;

public class ExecutionContext {
    private readonly Dictionary<string, Value> _constants;
    private readonly Dictionary<string, Value> _variables;
    private readonly List<Result> _results;

    public IReadOnlyDictionary<string, Value> Constants => _constants;
    public Dictionary<string, Value> Variables => _variables;
    public IReadOnlyList<Result> Results => _results;

    public ExecutionContext(IEnumerable<Constant> constants) {
        this._constants = constants.ToDictionary(x => x.Name, x => x.Value);
        this._variables = new();
        this._results = new();
    }

    public void AddResult(Result result) {
        _results.Add(result);
    }

    public void Clear() {
        _results.Clear();
        _variables.Clear();
    }
}
