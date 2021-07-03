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
            ExecutionUtils.AssertEquals( 2, "sqrt(4)" );
            ExecutionUtils.AssertEquals(2, "sqrt 4");
            ExecutionUtils.AssertEquals(4, "sqrt(4^2)");
        }

        [Test]
        public void TestComplexFunctions() {
            ExecutionUtils.AssertEquals(2, "log10 100");
            ExecutionUtils.AssertEquals(2, "log_10 100");
            ExecutionUtils.AssertEquals(2, "log(100, 10)");
        }
    }
}
