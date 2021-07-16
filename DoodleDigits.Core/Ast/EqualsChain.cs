using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast {
    public class EqualsChain : Expression {
        public enum EqualsType {
            Equals,
            NotEquals,
        }

        public static EqualsType GetTypeFromToken(TokenType token) {
            return token switch {
                TokenType.Equals => EqualsType.Equals,
                TokenType.NotEquals => EqualsType.NotEquals,
                _ => throw new ArgumentOutOfRangeException(nameof(token), token, null)
            };
        }

        public Expression[] Values { get; }
        public EqualsType[] EqualTypes { get; }

        public EqualsChain(IEnumerable<Expression> entries, IEnumerable<EqualsType> equalTypes, Range position) : base(position) {
            Values = entries.ToArray();
            EqualTypes = equalTypes.ToArray();

            if (Values.Length - 1 != EqualTypes.Length) {
                throw new ArgumentException("Length of types and entries don't match", nameof(equalTypes));
            }
        }

        public EqualsChain(IEnumerable<Expression> entries, IEnumerable<EqualsType> equalTypes) : this(entries, equalTypes, 0..0) {}

        public override Range FullPosition {
            get {
                Range fullPosition = Position;
                foreach (Expression argument in Values) {
                    fullPosition = Utils.Join(fullPosition, argument.FullPosition);
                }

                return fullPosition;
            }
        }

        public override bool Equals(AstNode other) {
            if (other is not EqualsChain otherEqChain) {
                return false;
            }

            if (otherEqChain.Values.Length != Values.Length) {
                return false;
            }

            for (int i = 0; i < Values.Length; i++) {
                if (Values[i].Equals(otherEqChain.Values[i]) == false) {
                    return false;
                }
            }

            for (int i = 0; i < EqualTypes.Length; i++) {
                if (EqualTypes[i] != otherEqChain.EqualTypes[i]) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Values.Length; i++) {
                sb.Append(Values[i].ToString());
                if (i < Values.Length - 1) {
                    sb.Append($" {(EqualTypes[i] == EqualsType.Equals ? "=" : "!=")} ");
                }
            }

            return sb.ToString();
        }
    }
}
