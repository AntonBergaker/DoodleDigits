

using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;
public class FunctionValue : Value {
    public string Identifier { get; }
    public string[] ArgumentNames { get; }
    public Expression Implementation { get; }

    public FunctionValue(string identifier, string[] argumentNames, Expression implementation, ValueTriviality triviality) : base(triviality) {
        Identifier = identifier;
        ArgumentNames = argumentNames;
        Implementation = implementation;
    }

    public override Value Clone(ValueTriviality? triviality = null) {
        return new FunctionValue(Identifier, ArgumentNames, Implementation, triviality ?? Triviality);
    }

    public override bool Equals(Value? other) {
        if (other is not FunctionValue otherFunction) {
            return false;
        }
        return otherFunction.Implementation.Equals(Implementation);
    }

    public override int GetHashCode() {
        return Implementation.GetHashCode();
    }

    public override string ToString() {
        return Implementation.ToString();
    }
}
