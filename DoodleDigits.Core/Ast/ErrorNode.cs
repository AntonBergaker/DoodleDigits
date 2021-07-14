using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
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
