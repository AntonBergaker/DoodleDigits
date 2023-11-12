using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Parsing;

namespace DoodleDigits.Core;

public class CalculationResult {
    public readonly Result[] Results;
    public CalculationResult(Result[] results) {
        Results = results;
    }
}

public class Calculator {

    private readonly Executor _executor;
    private readonly Parser _parser;

    public CalculatorSettings Settings { get; set; }

    /// <summary>
    /// Creates a new calculator with the the functions found inside <see cref="FunctionLibrary"/> and constants from <see cref="ConstantLibrary"/>
    /// </summary>
    public Calculator() : this(FunctionLibrary.Functions, ConstantLibrary.Constants) {

    }

    /// <summary>
    /// Creates a new calculator with the provided functions and constants
    /// </summary>
    /// <param name="functions">Functions to use</param>
    /// <param name="constants">Constants to use</param>
    public Calculator(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) {
        var functionData = functions as FunctionData[] ?? functions.ToArray();
        _executor = new Executor(functionData, constants);
        _parser = new Parser(functionData);
        Settings = new();
    }

    /// <summary>
    /// Calculates the results of the given string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>A result of the calculation</returns>
    public CalculationResult Calculate(string input) {
        ParseResult parseResult = _parser.Parse(input);
        ExecutionResult executionResult = _executor.Execute(parseResult.Root, Settings);

        List<Result> results = new();
        results.AddRange(executionResult.Results);
        results.AddRange(parseResult.Errors.Select(error => new ResultError(error.Message, error.Position)));

        results.Sort((a, b) => a.Position.End.GetOffset(input.Length) - b.Position.End.GetOffset(input.Length));

        return new CalculationResult(results.ToArray());
    }
}
