using DoodleDigits.Core;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions;
using NUnit.Framework;
using Rationals;

namespace UnitTests.Execution;
static class ExecutionTestUtils {

    public static void AssertEqual(Rational expected, string input, CalculatorSettings? settings = null) {
        AssertEqual(new RealValue(expected), input, settings);
    }

    public static void AssertEqual(bool expected, string input, CalculatorSettings? settings = null) {
        AssertEqual(new BooleanValue(expected), input, settings);
    }

    public static void AssertEqual(Value expected, string input, CalculatorSettings? settings = null) {
        var results = CalculateString(input, settings);

        foreach (Result result in results.Results) {
            if (result is ResultError error) {
                Assert.Fail($"Error in {input}.\n{error.Error} at {error.Position} (\"{input[error.Position]}\")");
            }
        }

        ResultValue last = results.Results.OfType<ResultValue>().Last();
        
        Assert.AreEqual(expected, last.Value);
    }

    public static void AssertEqual(string expected, string input) {
        Value expectedValue = CalculateValueString(expected);
        Value actualValue = CalculateValueString(input);
        Assert.AreEqual(expectedValue, actualValue);
    }

    public static CalculationResult CalculateString(string input, CalculatorSettings? settings = null) {
        Calculator calculator = new(FunctionLibrary.Functions, ConstantLibrary.Constants);
        if (settings != null) {
            calculator.Settings = settings;
        }
        return calculator.Calculate(input);
    }

    public static Value CalculateValueString(string input, CalculatorSettings? settings = null) {
        var result = CalculateString(input, settings);

        var last = result.Results.OfType<ResultValue>().LastOrDefault();
        if (last is not ResultValue rv) {
            Assert.Fail($"Input {input} did not return a result");
            throw new Exception();
        }

        if (rv.Value is UndefinedValue) {
            Assert.Fail($"Input {input} resulted in an undefined value");
            throw new Exception();
        }

        return rv.Value;
    }
}
