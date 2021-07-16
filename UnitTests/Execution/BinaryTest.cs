using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class BinaryTest {
        
        [Test]
        public void TestSingles() {
            
            ExecutionUtils.AssertEqual(10, "5 + 5");

            ExecutionUtils.AssertEqual(4, "2 ^ 2");
        }

        [Test]
        public void TestOrderOfOperations() {
            ExecutionUtils.AssertEqual(7, "1 + 2 * 3");
        }

        [Test]
        public void TestUnary() {
            ExecutionUtils.AssertEqual(-2, "2 + -4");

            ExecutionUtils.AssertEqual(6, "2 + -(-4)");
        }

    }
}
