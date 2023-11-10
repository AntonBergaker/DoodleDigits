namespace DoodleDigits.Core.Tokenizing;
class TokenReader {
    private readonly Token[] _tokens;
    private int _index;

    public TokenReader(Token[] tokens) {
        this._tokens = tokens;
        _index = 0;

        int lastIndex = tokens.LastOrDefault()?.Position.End.Value ?? 0;
        _eofToken = new Token("eof", TokenType.EndOfFile, lastIndex..lastIndex);
    }

    public bool ReachedEnd => _index >= _tokens.Length;
    public int Position => Math.Min(_index, _tokens.Length);

    private Token _eofToken;

    private Token SafeRead(int index) {
        if (index >= _tokens.Length) {
            return _eofToken;
        }

        return _tokens[index];
    }

    public Token Peek(bool skipNewLine = true) {
        int tempIndex = _index;
        while (skipNewLine && SafeRead(tempIndex).Type == TokenType.NewLine) {
            tempIndex++;
        }
        return SafeRead(tempIndex);
    }

    public Token Read(bool skipNewLine = true) {
        while (skipNewLine && SafeRead(_index).Type == TokenType.NewLine) {
            _index++;
        }
        return SafeRead(_index++);
    }

    public void Skip(bool skipNewLine = true) {
        while (skipNewLine && SafeRead(_index).Type == TokenType.NewLine) {
            _index++;
        }

        _index++;
    }

}
