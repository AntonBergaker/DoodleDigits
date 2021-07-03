using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core {
    public class Tokenizer {
        private int index;
        private string input;

        public Tokenizer() {
            index = 0;
            input = null!;
        }

        private static readonly Dictionary<char, TokenType> UniqueTokens = new Dictionary<char, TokenType> {
            {'+', TokenType.Add },
            {'-', TokenType.Subtract },
            {'*', TokenType.Multiply },
            {'/', TokenType.Divide },
            {'(', TokenType.ParenthesisOpen},
            {')', TokenType.ParenthesisClose},
            {',', TokenType.Comma},
            {'%', TokenType.Modulus},
            {'^', TokenType.Power},
        };

        public Token[] Tokenize(string input) {
            this.input = input;
            index = 0;

            List<Token> tokens = new();

            int lastIndex = -1;

            while (CanRead) {
                if (lastIndex == index) {
                    throw new Exception("The indexer was not incremented last loop");
                }
                lastIndex = index;
                Token? token = TokenizeOneInternal();
                if (token != null) {
                    tokens.Add(token);
                }
            }

            return tokens.ToArray();
        }

        public Token? TokenizeOne(string input, int index = 0) {
            this.input = input;
            this.index = index;
            if (CanRead == false) {
                return new Token("eof", TokenType.EndOfFile);
            }

            return TokenizeOneInternal();
        }

        private Token? TokenizeOneInternal() {
            char c = PeekOne();
            int startIndex = index;

            if (c == '\n') {
                index++;
                return new Token("\n", TokenType.NewLine, startIndex..startIndex);
            }
            if (char.IsWhiteSpace(c)) {
                index++;
                return null;
            }
            if (TryReadNumber()) {
                return new Token(input[startIndex..index], TokenType.Number, startIndex..index);
            }
            if (UniqueTokens.TryGetValue(c, out TokenType tokenType)) {
                index++;
                return new Token(c.ToString(), tokenType, startIndex..startIndex);
            }
            if (IsIdentifierFirstLetter(c)) {
                JumpUntilFalse(x => IsIdentifierLetter(x));
                return new Token(input[startIndex..index], TokenType.Identifier, startIndex..index);
            }

            index++;
            return new Token(c.ToString(), TokenType.Unknown, startIndex..startIndex);
        }

        private bool CanRead => index < input.Length;
        
        private bool IsIdentifierFirstLetter(char character) {
            return char.IsLetter(character) || character == '_';
        }
        private bool IsIdentifierLetter(char character) {
            return IsIdentifierFirstLetter(character) || char.IsDigit(character);
        }

        
        private bool TryReadNumber() {
            int startIndex = index;

            bool TryReadNumberInternal() {

                bool usedComma = false;
                bool hasDigit = false;

                while (CanRead) {
                    char c = ReadOne();
                    if (c == '.') {
                        if (usedComma) {
                            index--;
                            return false;
                        }

                        usedComma = true;
                    }
                    else if (char.IsDigit(c)) {
                        hasDigit = true;
                    }
                    else if (c == '_') {
                    }
                    else {
                        index--;
                        break;
                    }
                }

                return hasDigit;
            }

            bool result = TryReadNumberInternal();
            if (result == false) {
                index = startIndex;
            }

            return result;
        }

        private char PeekOne() {
            return input[index];
        }

        private char ReadOne() {
            return input[index++];
        }

        private void JumpUntilFalse(Func<char, bool> testFunction) {
            while (CanRead && testFunction(input[index])) {
                index++;
            }
        }

        private string Read(int count) {
            string result = input[index.. (index + count)];
            index += count;
            return result;
        }
    }
}
