using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class VariableTest {
        [Test]
        public void TestVariableAssignment() {

            ExecutionTestUtils.AssertEqual(3, "x = 3, x");

            ExecutionTestUtils.AssertEqual(6, "x = 1, y = x + 5, y");

        }

    }
}
