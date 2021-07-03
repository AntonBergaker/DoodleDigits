using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
    public abstract class AstNode : IEquatable<AstNode> {
        public Range Position { get; }

        protected AstNode(Range position) {
            Position = position;
        }

        bool IEquatable<AstNode>.Equals(AstNode? other) {
            if (other == null) {
                return false;
            }
            return this.Equals(other);
        }

        public abstract bool Equals(AstNode other);

        public abstract override string ToString();
    }
}
