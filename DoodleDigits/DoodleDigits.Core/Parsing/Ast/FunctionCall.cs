namespace DoodleDigits.Core.Parsing.Ast;
public class FunctionCall : Expression {
    public string Identifier { get; }
    public Expression[] Arguments { get; }

    public FunctionCall(string identifier, IEnumerable<Expression> arguments, Range position) : base(position) {
        Identifier = identifier;
        Arguments = arguments.ToArray();
    }

    public FunctionCall(string identifier, IEnumerable<Expression> arguments) : this(identifier, arguments, 0..0) { }

    public FunctionCall(string identifier, params Expression[] arguments) : this(identifier, (IEnumerable<Expression>)arguments) {}


    public override bool Equals(AstNode other) {
        if (other is not FunctionCall function) {
            return false;
        }

        if (function.Arguments.Length != Arguments.Length) {
            return false;
        }

        for (int i = 0; i < Arguments.Length; i++) {
            if (Arguments[i].Equals(function.Arguments[i]) == false) {
                return false;
            }
        }

        return true;
    }

    public override string ToString() {
        return $"{Identifier}({string.Join(", ", Arguments.Select(x => x.ToString()))})";
    }
}
