using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core {
    public class Tokenizer {
        private int index;
        private string input;


        private static readonly Dictionary<char, string[]> IdentifyTokens;

        static Tokenizer() {

            Dictionary<char, List<string>> charDictionary = new();

            foreach (KeyValuePair<string, TokenType> pair in Token.Tokens) {
                char c = pair.Key[0];

                List<string>? list = charDictionary.GetValueOrDefault(c);
                if (list == null) {
                    charDictionary[c] = list = new List<string>();
                }

                list.Add(pair.Key);
                list.Sort((a, b) => b.Length - a.Length);
            }

            IdentifyTokens = charDictionary.ToDictionary(
                x => x.Key, 
                x => x.Value.ToArray());
        }

        public Tokenizer() {
            index = 0;
            input = null!;
        }

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
                return null;
            }
            if (char.IsWhiteSpace(c)) {
                index++;
                return null;
            }
            if (TryReadNumber()) {
                return new Token(input[startIndex..index], TokenType.Number, startIndex..index);
            }
            if (IdentifyTokens.TryGetValue(c, out string[]? tokensWithChar)) {
                foreach (string str in tokensWithChar) {
                    int endIndex = index + str.Length;
                    if (input.Length >= endIndex && input[index..endIndex] == str) {
                        index += str.Length;
                        return new Token(str, Token.Tokens[str], startIndex..index);
                    }
                }
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
