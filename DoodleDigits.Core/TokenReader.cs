using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core {
    class TokenReader {
        private readonly Token[] tokens;
        private int index;

        public TokenReader(Token[] tokens) {
            this.tokens = tokens;
            index = 0;
        }

        public bool ReachedEnd => index >= tokens.Length;
        public int Position => Math.Min(index, tokens.Length);

        private Token EofToken => new Token("eof", TokenType.EndOfFile, tokens.Length..tokens.Length);

        private Token SafeRead(int index) {
            if (index >= tokens.Length) {
                return EofToken;
            }

            return tokens[index];
        }

        public Token Peek(bool skipNewLine = true) {
            int tempIndex = index;
            while (skipNewLine && SafeRead(tempIndex).Type == TokenType.NewLine) {
                tempIndex++;
            }
            return SafeRead(tempIndex);
        }

        public Token Read(bool skipNewLine = true) {
            while (skipNewLine && SafeRead(index).Type == TokenType.NewLine) {
                index++;
            }
            return SafeRead(index++);
        }

        public void Skip(bool skipNewLine = true) {
            while (skipNewLine && SafeRead(index).Type == TokenType.NewLine) {
                index++;
            }

            index++;
        }

    }
}
