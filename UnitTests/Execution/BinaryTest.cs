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

        [Test]
        public void TestEquals() {
            ExecutionUtils.AssertEquals(true, "5 = 5");
            ExecutionUtils.AssertEquals(false, "5 = 4");
            ExecutionUtils.AssertEquals(true, "x = 5, x = 5");
            ExecutionUtils.AssertEquals(false, "pi = 5");
            ExecutionUtils.AssertEquals(true, "a = pi*2, pi = a/2");

            ExecutionUtils.AssertEquals(5, "x = 5");
        }

        [Test]
        public void TestNotEquals() {
            ExecutionUtils.AssertEquals(false, "5 != 5");
            ExecutionUtils.AssertEquals(true, "5 != 4");
            
            ExecutionUtils.AssertEquals(true, "5 > 4 != 5 < 4");
        }
    }
}
