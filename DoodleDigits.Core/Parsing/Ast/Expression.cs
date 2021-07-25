using System;

namespace DoodleDigits.Core.Parsing.Ast {
    public abstract class Expression : AstNode {
        protected Expression(Range position) : base(position) { }
    }
}
