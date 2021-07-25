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
        BooleanOr,
        BooleanAnd,
        BooleanXor,
        Exclamation,
        ShiftLeft,
        ShiftRight,
        BitwiseOr,
        BitwiseAnd,
        BitwiseXor,
        NewLine,
    }

    public class Token : IEquatable<Token> {

        public static Dictionary<string, TokenType> Tokens;
        private static Dictionary<TokenType, string> tokenToString;

        static Token() {
            var tokens = new (string token, TokenType type)[] {             
                ("+", TokenType.Add),
                ("-", TokenType.Subtract),
                ("*", TokenType.Multiply),
                ("/", TokenType.Divide),
                ("(", TokenType.ParenthesisOpen),
                (")", TokenType.ParenthesisClose),
                (",", TokenType.Comma),
                ("%", TokenType.Modulus),
                ("^", TokenType.Power),
                ("=", TokenType.Equals),
                ("!=", TokenType.NotEquals),
                ("<=", TokenType.LessOrEqualTo),
                ("<", TokenType.LessThan),
                (">=", TokenType.GreaterOrEqualTo),
                (">", TokenType.GreaterThan),
                ("&&", TokenType.BooleanAnd),
                ("and", TokenType.BooleanAnd),
                ("||", TokenType.BooleanOr),
                ("or", TokenType.BooleanOr),
                ("^^", TokenType.BooleanXor),
                ("!", TokenType.Exclamation),
                ("<<", TokenType.ShiftLeft),
                (">>", TokenType.ShiftRight),
                ("|", TokenType.BitwiseOr),
                ("xor", TokenType.BitwiseXor),
                ("&", TokenType.BitwiseAnd),
            };

            Tokens = tokens.ToDictionary(x => x.token, x => x.type);

            tokenToString = new Dictionary<TokenType, string>();
            foreach (var token in tokens) {
                if (tokenToString.ContainsKey(token.type)) {
                    continue;
                }
                tokenToString.Add(token.type, token.token);
            }
        }


        public static string StringForTokenType(TokenType type) {
            return tokenToString[type];
        }


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
