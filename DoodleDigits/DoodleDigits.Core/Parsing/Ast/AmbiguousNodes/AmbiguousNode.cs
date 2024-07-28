
namespace DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;

/// <summary>
/// This node is ambiguous and can mean many different things, depending on the context it is used.
/// </summary>
public abstract class AmbiguousNode : Expression {
    protected AmbiguousNode(Range position) : base(position) {
    }
}
