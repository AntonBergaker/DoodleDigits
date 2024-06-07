namespace DoodleDigits.Core.Parsing.Ast;
public class VectorDeclaration : Expression {
    public readonly Expression[] Expressions;

    /// <summary>
    /// Convenient constructor for tests
    /// </summary>
    /// <param name="expressions"></param>
    public VectorDeclaration(params Expression[] expressions) : this(expressions, 0..0) { }

    public VectorDeclaration(IEnumerable<Expression> expressions, Range position) : base(position) {
        Expressions = expressions.ToArray();
    }

    public override bool Equals(AstNode other) {
        if (other is not VectorDeclaration otherVector) {
            return false;
        }

        return Enumerable.SequenceEqual(Expressions, otherVector.Expressions);
    }

    public override string ToString() {
        return $"({string.Join(", ", Expressions.Select(x => x.ToString()))})";
    }
}
