using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core; 
public class Tokenizer {
    private int index;
    private string input;

    private static readonly Dictionary<char, string[]> IdentifyTokens;

    private static readonly HashSet<string> AllEmoji = new(EmojiList.AllEmoji);
    private static readonly int EmojiMaxLength = EmojiList.AllEmoji.Select(x => x.Length).Max();

    static Tokenizer() {

        Dictionary<char, List<string>> charDictionary = new();

        foreach (KeyValuePair<string, TokenType> pair in Token.SymbolTokens) {
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
            return new Token("\n", TokenType.NewLine, startIndex..startIndex);
        }
        if (char.IsWhiteSpace(c)) {
            index++;
            return null;
        }
        if (TryReadNumber()) {
            return new Token(input[startIndex..index].Trim(), TokenType.Number, startIndex..index);
        }
        if (IdentifyTokens.TryGetValue(c, out string[]? tokensWithChar)) {
            foreach (string str in tokensWithChar) {
                int endIndex = index + str.Length;
                if (input.Length >= endIndex && input[index..endIndex] == str) {
                    index += str.Length;
                    return new Token(str, Token.SymbolTokens[str], startIndex..index);
                }
            }
        }
        if (IsIdentifierFirstCharacter(index, out int readLength)) {
            index += readLength;
            while (CanRead && IsIdentifierCharacter(index, out readLength)) {
                index+= readLength;
            }
            string identifier = input[startIndex..index];

            if (Token.TryTokenTypeForString(identifier, out TokenType type)) {
                return new Token(identifier, type, startIndex..index);
            }

            return new Token(identifier, TokenType.Identifier, startIndex..index);
        }

        index++;
        return new Token(c.ToString(), TokenType.Unknown, startIndex..startIndex);
    }

    private bool CanRead => index < input.Length;
    
    private bool IsIdentifierFirstCharacter(int index, out int length) {
        char @char = input[index];
        if (char.IsLetter(@char) || @char == '_') {
            length = 1;
            return true;
        }
        // Early return
        if (char.IsWhiteSpace(@char) || char.IsDigit(@char)) {
            length = 0;
            return false;
        }

        // Compare to every length of emoji, starting with the longest
        for (int i = Math.Min(input.Length, index + EmojiMaxLength); i > index; i--) {
            if (AllEmoji.Contains(input[index..i])) {
                length = i - index;
                return true;
            }
        }

        length = 0;
        return false;
    }
    private bool IsIdentifierCharacter(int index, out int length) {
        char c = input[index];
        if (char.IsDigit(c)) {
            length = 1;
            return true;
        }
        if (c == '\'') {
            length = 1;
            return true;
        }

        return IsIdentifierFirstCharacter(index, out length);
    }

    
    private bool TryReadNumber() {
        int startIndex = index;

        bool TryReadNumberInternal(string allowedDigits) {

            bool usedComma = false;
            bool hasDigit = false;
            bool lastWasSpace = false;

            while (CanRead) {
                char c = ReadOne();
                if (c == ' ') {
                    if (lastWasSpace) {
                        break;
                    }

                    lastWasSpace = true;
                    continue;
                }
                
                lastWasSpace = false;

                if (c == '.') {
                    if (usedComma) {
                        index--;
                        return false;
                    }

                    usedComma = true;
                }
                else if (allowedDigits.Contains(c)) {
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

        bool result = false;

        if (PeekOne() == '0') {
            string start = Peek(2);
            if (start == "0x") {
                index += 2;
                result = TryReadNumberInternal("0123456789abcdefABCDEF");
            } else if (start == "0b") {
                index += 2;
                result = TryReadNumberInternal("01");
            }
        }

        if (result == false) {
            result = TryReadNumberInternal("0123456789");
            // See if it's in scientific notation
            if (index < input.Length && input[index] is 'E' or 'e' or 'ᴇ') {
                int preScienceIndex = index;
                index++;
                if (index < input.Length && input[index] is '+' or '-') {
                    index++;
                }

                if (index >= input.Length || char.IsNumber(input[index]) == false) {
                    index = preScienceIndex;
                }
                else {
                    JumpUntilFalse(x => char.IsNumber(x));
                }

            }
        }

        if (result == false) {
            index = startIndex;
        }

        return result;
    }

    private char PeekOne() {
        return input[index];
    }

    private string Peek(int count) {
        return input[index.. Math.Min(index + count, input.Length)];
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
