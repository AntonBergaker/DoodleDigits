namespace DoodleDigits.Core.Execution.Results;
public abstract class Result {
    public Range Position { get; }

    protected Result(Range position) {
        Position = position;
    }

}
