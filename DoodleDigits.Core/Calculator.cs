using System.Collections.Generic;
using System.Linq;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Parsing;

namespace DoodleDigits.Core {

    public class CalculationResult {
        public readonly Result[] Results;
        public CalculationResult(Result[] results) {
            Results = results;
        }
    }

    public class Calculator {

        private readonly Executor executor;
        private readonly Parser parser;

        public Calculator(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) {
            var functionData = functions as FunctionData[] ?? functions.ToArray();
            executor = new Executor(functionData, constants);
            parser = new Parser(functionData.SelectMany(x => x.Names));
        }

        public CalculationResult Calculate(string input) {
            ParseResult parseResult = parser.Parse(input);
            ExecutionResult executionResult = executor.Execute(parseResult.Root);

            List<Result> results = new();
            results.AddRange(executionResult.Results);
            results.AddRange(parseResult.Errors.Select(error => new ResultError(error.Message, error.Position)));

            results.Sort((a, b) => a.Position.Start.GetOffset(input.Length) - b.Position.Start.GetOffset(input.Length));

            return new CalculationResult(results.ToArray());
        }
    }
}
