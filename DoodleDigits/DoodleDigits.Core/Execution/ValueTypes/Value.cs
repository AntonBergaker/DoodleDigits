using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;

public enum ValueTriviality {
    Unknown,
    Trivial,
    TrivialSideEffect,
    NonTrivial,
}

public abstract class Value : IEquatable<Value> {
    public abstract override string ToString();

    public ValueTriviality Triviality { get; }

    protected Value(ValueTriviality triviality) {
        Triviality = triviality;
    }

    public virtual Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }

    public virtual Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }
    public virtual Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }

    public virtual Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }

    public virtual Value? TryModulus(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }
    public virtual Value? TryPower(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        return null;
    }

    public abstract bool Equals(Value? other);

    public override bool Equals(object? obj) {
        if (obj is not Value val) {
            return false;
        }
        return Equals(val);
    }

    public abstract override int GetHashCode();
    public abstract Value Clone(ValueTriviality? triviality = null);
}
