using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
    public class NumberLiteral : Expression {
        public string Number { get; }

        public override Range FullPosition => Position;

        public NumberLiteral(string number, Range position) : base(position) {
            Number = number;
        }

        public NumberLiteral(string number) : this(number, 0..0) { }

        public override bool Equals(AstNode other) {
            if (other is not NumberLiteral nl) {
                return false;
            }

            return nl.Number == Number;
        }

        public override string ToString() {
            return Number;
        }
    }
}
