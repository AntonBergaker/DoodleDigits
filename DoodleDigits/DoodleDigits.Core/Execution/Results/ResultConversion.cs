using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution.Results;
public class ResultConversion : Result {
    public enum ConversionType {
        TypeChange,
        Rounding
    }

    public Value PreviousValue { get; }
    public Value NewValue { get; }
    public ConversionType Type { get; }

    public ResultConversion(Value previousValue, Value newValue, ConversionType type, Range position) : base(position) {
        PreviousValue = previousValue;
        NewValue = newValue;
        Type = type;
    }

    public override string ToString() {
        return $"Converted {PreviousValue} to {NewValue}";
    }
}
