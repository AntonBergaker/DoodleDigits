using System;

namespace DoodleDigits.Core.Parsing.Ast {
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

        public abstract bool Equals(AstNode other);

        public abstract override string ToString();
    }
}
