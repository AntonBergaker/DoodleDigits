namespace DoodleDigits.Core.Parsing.Ast;
public class VectorDecleration : Expression {
    public readonly Expression[] Expressions;

    /// <summary>
    /// Convinient construtor for tests
    /// </summary>
    /// <param name="expressions"></param>
    public VectorDecleration(params Expression[] expressions) : this(expressions, 0..0) { }

    public VectorDecleration(IEnumerable<Expression> expressions, Range position) : base(position) {
        Expressions = expressions.ToArray();
    }

    public override bool Equals(AstNode other) {
        if (other is not VectorDecleration otherVector) {
            return false;
        }

        return Enumerable.SequenceEqual(Expressions, otherVector.Expressions);
    }

    public override string ToString() {
        return $"({string.Join(", ", Expressions.Select(x => x.ToString()))})";
    }
}
