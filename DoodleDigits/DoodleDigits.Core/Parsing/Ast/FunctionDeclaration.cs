namespace DoodleDigits.Core.Parsing.Ast;

public class FunctionDeclaration : AstNode {
    public string Identifier { get; }
    public string[] ArgumentNames { get; }
    public Expression Implementation { get; }

    public FunctionDeclaration(string identifier, IEnumerable<string> argumentNames, Expression implementation) : this(identifier, argumentNames, implementation, 0..0) { }

    public FunctionDeclaration(string identifier, IEnumerable<string> argumentNames, Expression implementation, Range position) : base(position) {
        Identifier = identifier;
        ArgumentNames = argumentNames.ToArray();
        Implementation = implementation;
    }

    public override bool Equals(AstNode other) {
        if (other is not FunctionDeclaration function) {
            return false;
        }

        if (Implementation.Equals(function.Implementation) == false) {
            return false;
        }

        if (Enumerable.SequenceEqual(ArgumentNames, function.ArgumentNames) == false) {
            return false;
        }

        return true;
    }

    public override string ToString() {
        return $"{Identifier}({string.Join(", ", ArgumentNames)}) = {Implementation}";
    }
}
