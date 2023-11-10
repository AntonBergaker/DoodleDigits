namespace DoodleDigits.Core.Execution.Results;
public class ResultError : Result {
    public string Error { get; }

    public ResultError(string error, Range position) : base(position) {
        Error = error;
    }
}
