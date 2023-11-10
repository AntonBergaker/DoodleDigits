namespace DoodleDigits.Core.Parsing.Ast;
public abstract class AstNode : IEquatable<AstNode> {

    protected AstNode(Range position) {
        Position = position;
    }

    bool IEquatable<AstNode>.Equals(AstNode? other) {
        if (other == null) {
            return false;
        }
        return this.Equals(other);
    }

    /// <summary>
    /// Like position, but also includes its children
    /// </summary>
    public Range Position { get; set; }

    public override bool Equals(object? obj) {
        if (obj is not AstNode node) {
            return false;
        }

        return Equals(node);
    }

    public override int GetHashCode() {
        return 0;
    }

    public abstract bool Equals(AstNode other);

    public abstract override string ToString();
}
