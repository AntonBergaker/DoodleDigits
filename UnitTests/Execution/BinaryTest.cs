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

        [Test]
        public void TestEquals() {
            ExecutionUtils.AssertEqual(true, "5 = 5");
            ExecutionUtils.AssertEqual(false, "5 = 4");
            ExecutionUtils.AssertEqual(true, "x = 5, x = 5");
            ExecutionUtils.AssertEqual(false, "pi = 5");
            ExecutionUtils.AssertEqual(true, "a = pi*2, pi = a/2");

            ExecutionUtils.AssertEqual(5, "x = 5");
        }

        [Test]
        public void TestNotEquals() {
            ExecutionUtils.AssertEqual(false, "5 != 5");
            ExecutionUtils.AssertEqual(true, "5 != 4");
            
            ExecutionUtils.AssertEqual(true, "5 > 4 != 5 < 4");
        }
    }
}
