using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using NUnit.Framework;

namespace UnitTests.Execution {
    static class ExecutionUtils {

        public static void AssertEquals(double expected, string input) {

            Executor executor = new(FunctionLibrary.Functions);

            var result = executor.Calculate(input);

            Assert.AreEqual(expected.ToString(), result.Results[0]);

        }

    }
}
