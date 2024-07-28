

using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;
public class FunctionValue : Value {
    public string Identifier { get; }
    public string[] ArgumentNames { get; }
    public Expression Implementation { get; }

    public FunctionValue(string identifier, string[] argumentNames, Expression implementation, bool triviallyAchieved) : base(triviallyAchieved) {
        Identifier = identifier;
        ArgumentNames = argumentNames;
        Implementation = implementation;
    }

    public override Value Clone(bool? triviallyAchieved = null) {
        return new FunctionValue(Identifier, ArgumentNames, Implementation, triviallyAchieved ?? TriviallyAchieved);
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
