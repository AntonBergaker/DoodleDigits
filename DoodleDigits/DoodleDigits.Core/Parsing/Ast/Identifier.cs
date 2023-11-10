namespace DoodleDigits.Core.Parsing.Ast;
public class Identifier : Expression {
    public string Value { get; }

    public Identifier(string identifier, Range position) : base(position) {
        Value = identifier;
    }

    public Identifier(string identifier) : this(identifier, 0..0) {}

    public override bool Equals(AstNode other) {
        if (other is not Identifier identifier) {
            return false;
        }

        return Value == identifier.Value;
    }

    public override string ToString() {
        return Value;
    }
}
