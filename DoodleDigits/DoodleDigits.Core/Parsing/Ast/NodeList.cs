namespace DoodleDigits.Core.Parsing.Ast;
public class NodeList : AstNode {
    public List<AstNode> Nodes { get; }

    public NodeList(List<AstNode> nodes, Range position) : base(position) {
        Nodes = nodes;
    }

    public NodeList(List<AstNode> nodes) : this(nodes, 0..0) {}

    public override bool Equals(AstNode other) {
        if (other is not NodeList el) {
            return false;
        }

        if (Nodes.Count != el.Nodes.Count) {
            return false;
        }

        for (int i = 0; i < Nodes.Count; i++) {
            if (Nodes[i].Equals(el.Nodes[i]) == false) {
                return false;
            } 
        }

        return true;
    }

    public override string ToString() {
        return string.Join('\n', Nodes.Select(x => x.ToString()));
    }
}
