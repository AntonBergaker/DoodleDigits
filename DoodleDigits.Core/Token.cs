using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;

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
        Modulus,
        EndOfFile,
        Power,
        Unknown,
        Equals,
        NotEquals,
        LessOrEqualTo,
        LessThan,
        GreaterOrEqualTo,
        GreaterThan,
        Or,
        And,
        Exclamation
    }

    public class Token : IEquatable<Token> {

        public static readonly TwoWayDictionary<string, TokenType> Tokens = new() {
            { "+", TokenType.Add },
            { "-", TokenType.Subtract },
            { "*", TokenType.Multiply },
            { "/", TokenType.Divide },
            { "(", TokenType.ParenthesisOpen },
            { ")", TokenType.ParenthesisClose },
            { ",", TokenType.Comma },
            { "%", TokenType.Modulus },
            { "^", TokenType.Power },
            { "=", TokenType.Equals },
            { "!=", TokenType.NotEquals },
            { "<=", TokenType.LessOrEqualTo },
            { "<", TokenType.LessThan },
            { ">=", TokenType.GreaterOrEqualTo },
            { ">", TokenType.GreaterThan },
            { "&&", TokenType.And },
            { "||", TokenType.Or },
            { "!", TokenType.Exclamation},
        };
        

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
