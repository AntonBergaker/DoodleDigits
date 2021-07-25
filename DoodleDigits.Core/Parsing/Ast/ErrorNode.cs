using System;

namespace DoodleDigits.Core.Parsing.Ast {
    public class ErrorNode : Expression {

 

        public ErrorNode(Range position) : base(position) {
            
        }

        public ErrorNode() : this(0..0) {}


        public override bool Equals(AstNode other) {
            return other is ErrorNode;
        }

        public override Range FullPosition => Position;

        public override string ToString() {
            return "Error!";
        }
    }
}
