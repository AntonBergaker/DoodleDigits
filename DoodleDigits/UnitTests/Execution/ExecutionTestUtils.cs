using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing;
using NUnit.Framework;
using Rationals;

namespace UnitTests.Execution {
    static class ExecutionTestUtils {

        public static void AssertEqual(Rational expected, string input) {
            AssertEqual(new RealValue(expected), input);
        }

        public static void AssertEqual(bool expected, string input) {
            AssertEqual(new BooleanValue(expected), input);
        }

        public static void AssertEqual(Value expected, string input) {
            var results = CalculateString(input);

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

        public static CalculationResult CalculateString(string input) {
            Calculator calculator = new(FunctionLibrary.Functions, ConstantLibrary.Constants);

            return calculator.Calculate(input);
        }

        public static Value CalculateValueString(string input) {
            var result = CalculateString(input);

            ResultValue last = result.Results.OfType<ResultValue>().Last();
            if (last is not ResultValue rv) {
                Assert.Fail("Input did not return a result");
                throw new Exception();
            }

            if (rv.Value is UndefinedValue) {
                Assert.Fail("Input resulted in an undefined value");
                throw new Exception();
            }

            return rv.Value;
        }
    }
}
