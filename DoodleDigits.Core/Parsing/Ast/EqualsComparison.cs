using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing.Ast {

    public class EqualsComparison : Expression {
        public enum EqualsSign {
            Equals,
            NotEquals,
        }

        public class Builder : IEnumerable<Expression> {
            internal Range Position { get; private set; }

            public Builder(Expression lhs) : this(lhs, 0..0) { }

            public Builder(Expression lhs, Range position) {
                Expressions = new List<Expression>() { lhs };
                Signs = new List<EqualsSign>();
                Position = position;
            }

            public Builder Add(EqualsSign sign, Expression rhs) {
                Signs.Add(sign);
                Expressions.Add(rhs);

                return this;
            }

            public Builder Add(EqualsSign sign, Expression rhs, Range position) {
                Position = Utils.Join(Position, position);
                return Add(sign, rhs);
            }

            public EqualsComparison Build() {
                return new EqualsComparison(this);
            }

            internal List<Expression> Expressions { get; }
            internal List<EqualsSign> Signs { get; }

            // Only to make the thingy possible
            public IEnumerator<Expression> GetEnumerator() {
                throw new NotImplementedException();
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }


        public static EqualsSign GetTypeFromToken(TokenType token) {
            return token switch {
                TokenType.Equals => EqualsSign.Equals,
                TokenType.NotEquals => EqualsSign.NotEquals,
                _ => throw new ArgumentOutOfRangeException(nameof(token), token, null)
            };
        }

        public Expression[] Expressions { get; }
        public EqualsSign[] Signs { get; }

        public EqualsComparison(Expression lhs, EqualsSign sign, Expression rhs) : this(new Builder(lhs).Add(sign, rhs)) {
        }

        private EqualsComparison(Builder builder) : base(builder.Position) {
            Expressions = builder.Expressions.ToArray();
            Signs = builder.Signs.ToArray();
        }

        public override bool Equals(AstNode other) {
            if (other is not EqualsComparison otherEqChain) {
                return false;
            }

            if (otherEqChain.Expressions.Length != Expressions.Length) {
                return false;
            }

            for (int i = 0; i < Expressions.Length; i++) {
                if (Expressions[i].Equals(otherEqChain.Expressions[i]) == false) {
                    return false;
                }
            }

            for (int i = 0; i < Signs.Length; i++) {
                if (Signs[i] != otherEqChain.Signs[i]) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Expressions.Length; i++) {
                sb.Append(Expressions[i].ToString());
                if (i < Expressions.Length - 1) {
                    sb.Append($" {(Signs[i] == EqualsSign.Equals ? "=" : "!=")} ");
                }
            }

            return sb.ToString();
        }
    }
}
