using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core {
    public enum TokenType {
        Add,
        Subtract,
        Multiply,
        Divide,
        Identifier,
        Number,
        ParenthesisClose,
        ParenthesisOpen,
        Comma,
        NewLine,
        Modulus,
        EndOfFile,
        Power,
        Unknown,
    }

    public class Token : IEquatable<Token> {

        public Token(string content, TokenType type) {
            Content = content;
            Type = type;
            Position = 0..0;
        }

        public Token(string content, TokenType type, Range position) {
            Content = content;
            Type = type;
            Position = position;
        }

        public string Content { get; }
        public TokenType Type { get; }
        public Range Position { get; }

        public bool Equals(Token? other) {
            if (other == null) {
                return false;
            }

            return (Content == other.Content && Type == other.Type);
        }

        public override string ToString() {
            return $"{Content}({Type})";
        }
    }
}
