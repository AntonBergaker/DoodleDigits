using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
    public class ErrorNode : Expression {
        public readonly string? ErrorMessage;

        public ErrorNode(string message, Range position) : base(position) {
            ErrorMessage = message;
        }

        public ErrorNode(Range position) : base(position) {
            
        }

        public ErrorNode() : this(0..0) {}

        public ErrorNode(string message) : this(message, 0..0) {}

        public override bool Equals(AstNode other) {
            return other is ErrorNode;
        }

        public override string ToString() {
            return $"Error! ({ErrorMessage})";
        }
    }
}
