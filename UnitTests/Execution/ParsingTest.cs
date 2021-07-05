using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class ParsingTest {

        [Test]
        public void TestNumberParse() {
            ExecutionUtils.AssertEquals(5.123, "5.123");
            ExecutionUtils.AssertEquals(1_000_000, "1_000_000");
        }

    }
}
