using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core {
    class TokenReader {
        private readonly Token[] tokens;
        private int index;

        private Stack<int> states;

        public TokenReader(Token[] tokens) {
            this.tokens = tokens;
            index = 0;
            states = new Stack<int>();
        }

        public bool ReachedEnd => index >= tokens.Length;
        public int Position => Math.Min(index, tokens.Length);

        public Token Peek() {
            if (ReachedEnd) {
                return new Token("eof", TokenType.EndOfFile, tokens.Length..tokens.Length);
            }
            return tokens[index];
        }
        public Token Read() {
            if (ReachedEnd) {
                return new Token("eof", TokenType.EndOfFile, tokens.Length..tokens.Length);
            }
            return tokens[index++];
        }

        /// <summary>
        /// Saves the current state
        /// </summary>
        public void SaveState() {
            states.Push(index);
        }

        /// <summary>
        /// Ignores the state you pushed
        /// </summary>
        public void DiscardState() {
            states.Pop();
        }

        /// <summary>
        /// Sets index to what it was when you pushed
        /// </summary>
        public void RewindState() {
            index = states.Pop();
        }

        public void Skip() {
            index++;
        }
    }
}
