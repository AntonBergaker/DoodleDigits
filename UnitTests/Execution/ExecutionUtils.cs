using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using NUnit.Framework;

namespace UnitTests.Execution {
    static class ExecutionUtils {

        public static void AssertEquals(double expected, string input) {
            AssertEquals(new RealValue(expected), input);
        }

        public static void AssertEquals(bool expected, string input) {
            AssertEquals(new BooleanValue(expected), input);
        }

        public static void AssertEquals(Value expected, string input) {

            Executor executor = new(FunctionLibrary.Functions, ConstantLibrary.Constants);

            var results = executor.Calculate(input);

            foreach (Result result in results.Results) {
                if (result is ResultError error) {
                    Assert.Fail($"Error in {input}.\n{error.Error} at {error.Position} (\"{input[error.Position]}\")");
                }
            }

            Assert.AreEqual(expected.ToString(), results.Results.OfType<ResultValue>().Last().Value.ToString()!);
        }
    }
}
