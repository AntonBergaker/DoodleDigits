namespace DoodleDigits.Core.Parsing;
public class ParseError {
    public readonly Range Position;
    public readonly string Message;

    public ParseError(Range position, string message) {
        this.Position = position;
        this.Message = message;
    }
}
