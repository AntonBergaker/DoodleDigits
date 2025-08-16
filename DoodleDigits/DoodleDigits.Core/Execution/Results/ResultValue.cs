using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution.Results;
public class ResultValue : Result {
    public Value Value { get; }

    public ResultValue(Value value, Range position) : base(position) {
        Value = value;
    }

    public override string ToString() {
        return Value.ToString();
    }
}
