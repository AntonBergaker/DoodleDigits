using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
    public abstract class Expression : AstNode {
        protected Expression(Range position) : base(position) { }
    }
}
