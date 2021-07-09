using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class FunctionTest {
        [Test]
        public void TestSimpleFunctions() {
            ExecutionUtils.AssertEqual( 2, "sqrt(4)" );
            ExecutionUtils.AssertEqual(2, "sqrt 4");
            ExecutionUtils.AssertEqual(4, "sqrt(4^2)");
        }

        [Test]
        public void TestComplexFunctions() {
            ExecutionUtils.AssertEqual(2, "log10 100");
            ExecutionUtils.AssertEqual(2, "log_10 100");
            ExecutionUtils.AssertEqual(2, "log(100, 10)");

            ExecutionUtils.AssertEqual(2, "root2 4");
            ExecutionUtils.AssertEqual(2, "root_2 4");
            ExecutionUtils.AssertEqual(2, "root(4, 2)");

        }

        [Test]
        public void TestVariableParameterCount() {
            ExecutionUtils.AssertEqual(5, "max(1, 2, 3, 4, 5)");
            ExecutionUtils.AssertEqual(1, "min(1, 2, 3, 4, 5)");

            ExecutionUtils.AssertEqual(15, "max(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)");
            ExecutionUtils.AssertEqual(1, "min(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)");
        }
    }
}
