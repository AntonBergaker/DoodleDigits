using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using System.Diagnostics.CodeAnalysis;

namespace DoodleDigits.Core.Execution;

public class ExecutorContext {
    private readonly List<Dictionary<string, Value>> _variables;

    private readonly List<Result> _results;


    public IReadOnlyList<Result> Results => _results;
    public CalculatorSettings Settings { get; internal set; }

    public ExecutorContext(CalculatorSettings settings, Dictionary<string, Value> constants) {
        Settings = settings;
        _variables = new();

        PushVariableStack();

        foreach (var constant in constants) {
            AddVariable(constant.Key, constant.Value);
        }
        this._results = new();
    }

    public void AddResult(Result result) {
        _results.Add(result);
    }

    public void Clear() {
        _results.Clear();
        _variables.Clear();
    }

    public bool TryGetVariable(string identifier, [MaybeNullWhen(false)] out Value value) {
        for (int i = _variables.Count - 1; i >= 0; i--) {
            var stack = _variables[i];
            if (stack.TryGetValue(identifier, out var foundValue)) {
                value = foundValue;
                return true;
            }
        }

        value = null;
        return false;
    }

    public void AddVariable(string identifier, Value value) {
        _variables[^1].Add(identifier, value);
    }

    public void PushVariableStack() {
        _variables.Add(new());
    }

    public void PopVariableStack() {
        _variables.RemoveAt(_variables.Count-1);
    }
}
