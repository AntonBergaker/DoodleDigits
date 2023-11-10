using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Tokenizing;
public class Tokenizer {
    private int _index;
    private string _input;

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
        _index = 0;
        _input = null!;
    }

    public Token[] Tokenize(string input) {
        this._input = input;
        _index = 0;

        List<Token> tokens = new();

        int lastIndex = -1;

        while (CanRead) {
            if (lastIndex == _index) {
                throw new Exception("The indexer was not incremented last loop");
            }
            lastIndex = _index;
            Token? token = TokenizeOneInternal();
            if (token != null) {
                tokens.Add(token);
            }
        }

        return tokens.ToArray();
    }

    public Token? TokenizeOne(string input, int index = 0) {
        this._input = input;
        this._index = index;
        if (CanRead == false) {
            return new Token("eof", TokenType.EndOfFile);
        }

        return TokenizeOneInternal();
    }

    private Token? TokenizeOneInternal() {
        char c = PeekOne();
        int startIndex = _index;

        if (c == '\n') {
            _index++;
            return new Token("\n", TokenType.NewLine, startIndex..startIndex);
        }
        if (char.IsWhiteSpace(c)) {
            _index++;
            return null;
        }
        if (TryReadNumber()) {
            return new Token(_input[startIndex.._index].Trim(), TokenType.Number, startIndex.._index);
        }
        if (IdentifyTokens.TryGetValue(c, out string[]? tokensWithChar)) {
            foreach (string str in tokensWithChar) {
                int endIndex = _index + str.Length;
                if (_input.Length >= endIndex && _input[_index..endIndex] == str) {
                    _index += str.Length;
                    return new Token(str, Token.SymbolTokens[str], startIndex.._index);
                }
            }
        }
        if (IsIdentifierFirstCharacter(_index, out int readLength)) {
            _index += readLength;
            while (CanRead && IsIdentifierCharacter(_index, out readLength)) {
                _index+= readLength;
            }
            string identifier = _input[startIndex.._index];

            if (Token.TryTokenTypeForString(identifier, out TokenType type)) {
                return new Token(identifier, type, startIndex.._index);
            }

            return new Token(identifier, TokenType.Identifier, startIndex.._index);
        }

        _index++;
        return new Token(c.ToString(), TokenType.Unknown, startIndex..startIndex);
    }

    private bool CanRead => _index < _input.Length;
    
    private bool IsIdentifierFirstCharacter(int index, out int length) {
        char @char = _input[index];
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
        for (int i = Math.Min(_input.Length, index + EmojiMaxLength); i > index; i--) {
            if (AllEmoji.Contains(_input[index..i])) {
                length = i - index;
                return true;
            }
        }

        length = 0;
        return false;
    }
    private bool IsIdentifierCharacter(int index, out int length) {
        char c = _input[index];
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
        int startIndex = _index;

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
                        _index--;
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
                    _index--;
                    break;
                }
            }

            return hasDigit;
        }

        bool result = false;

        if (PeekOne() == '0') {
            string start = Peek(2);
            if (start == "0x") {
                _index += 2;
                result = TryReadNumberInternal("0123456789abcdefABCDEF");
            } else if (start == "0b") {
                _index += 2;
                result = TryReadNumberInternal("01");
            }
        }

        if (result == false) {
            result = TryReadNumberInternal("0123456789");
            // See if it's in scientific notation
            if (_index < _input.Length && _input[_index] is 'E' or 'e' or 'ᴇ') {
                int preScienceIndex = _index;
                _index++;
                if (_index < _input.Length && _input[_index] is '+' or '-') {
                    _index++;
                }

                if (_index >= _input.Length || char.IsNumber(_input[_index]) == false) {
                    _index = preScienceIndex;
                }
                else {
                    JumpUntilFalse(x => char.IsNumber(x));
                }

            }
        }

        if (result == false) {
            _index = startIndex;
        }

        return result;
    }

    private char PeekOne() {
        return _input[_index];
    }

    private string Peek(int count) {
        return _input[_index.. Math.Min(_index + count, _input.Length)];
    }

    private char ReadOne() {
        return _input[_index++];
    }

    private void JumpUntilFalse(Func<char, bool> testFunction) {
        while (CanRead && testFunction(_input[_index])) {
            _index++;
        }
    }

    private string Read(int count) {
        string result = _input[_index.. (_index + count)];
        _index += count;
        return result;
    }
}
