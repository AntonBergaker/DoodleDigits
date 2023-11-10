namespace DoodleDigits.Core.Parsing.Ast;
public class ExpressionList : AstNode {
    public List<Expression> Expressions { get; }

    public ExpressionList(List<Expression> expressions, Range position) : base(position) {
        Expressions = expressions;
    }

    public ExpressionList(List<Expression> expressions) : this(expressions, 0..0) {}

    public override bool Equals(AstNode other) {
        if (other is not ExpressionList el) {
            return false;
        }

        if (Expressions.Count != el.Expressions.Count) {
            return false;
        }

        for (int i = 0; i < Expressions.Count; i++) {
            if (Expressions[i].Equals(el.Expressions[i]) == false) {
                return false;
            } 
        }

        return true;
    }

    public override string ToString() {
        return string.Join('\n', Expressions.Select(x => x.ToString()));
    }
}
