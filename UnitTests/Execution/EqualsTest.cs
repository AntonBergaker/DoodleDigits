using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution {
    class EqualsTest {
        [Test]
        public void TestBinaryEquals() {
            ExecutionUtils.AssertEqual(true, "5 = 5");
            ExecutionUtils.AssertEqual(false, "5 = 4");
            ExecutionUtils.AssertEqual(true, "x = 5, x = 5");
            ExecutionUtils.AssertEqual(false, "pi = 5");
            ExecutionUtils.AssertEqual(true, "a = pi*2, pi = a/2");

            ExecutionUtils.AssertEqual(5, "x = 5");
        }

        [Test]
        public void TestBinaryNotEquals() {
            ExecutionUtils.AssertEqual(false, "5 != 5");
            ExecutionUtils.AssertEqual(true, "5 != 4");

            ExecutionUtils.AssertEqual(true, "5 > 4 != 5 < 4");
        }

        [Test]
        public void TestManyEquals() {
            ExecutionUtils.AssertEqual(true, "5 = 5 = 5");
            ExecutionUtils.AssertEqual(true, "5 = 5 = 5 = 5 = 5");
            ExecutionUtils.AssertEqual(false, "5 = 5 = 5 = 5 = 6");
            ExecutionUtils.AssertEqual(false, "5 = 5 = 6 = 5 = 5");
        }
    }
}
