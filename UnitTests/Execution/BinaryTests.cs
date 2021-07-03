using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class BinaryTests {
        
        [Test]
        public void TestSingles() {
            
            ExecutionUtils.AssertEquals(10, "5 + 5");

            ExecutionUtils.AssertEquals(4, "2 ^ 2");
        }

        [Test]
        public void TestOrderOfOperations() {

            ExecutionUtils.AssertEquals(7, "1 + 2 * 3");

        }

        [Test]
        public void TestUnary() {
            ExecutionUtils.AssertEquals(-2, "2 + -4");

            ExecutionUtils.AssertEquals(6, "2 + -(-4)");
        }
    }
}
