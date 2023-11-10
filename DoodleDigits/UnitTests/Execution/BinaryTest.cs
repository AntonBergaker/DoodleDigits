using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution; 
class BinaryTest {
    
    [Test]
    public void TestSingles() {
        
        ExecutionTestUtils.AssertEqual(10, "5 + 5");

        ExecutionTestUtils.AssertEqual(4, "2 ^ 2");
    }

    [Test]
    public void TestOrderOfOperations() {
        ExecutionTestUtils.AssertEqual(7, "1 + 2 * 3");
    }

    [Test]
    public void TestUnary() {
        ExecutionTestUtils.AssertEqual(-2, "2 + -4");

        ExecutionTestUtils.AssertEqual(6, "2 + -(-4)");
    }

    [Test]
    public void TestShift() {
        ExecutionTestUtils.AssertEqual(10, "5 << 1");
        ExecutionTestUtils.AssertEqual(5, "5 << 0");
        ExecutionTestUtils.AssertEqual(20, "5 << 2");

        ExecutionTestUtils.AssertEqual(25, "50 >> 1");
        ExecutionTestUtils.AssertEqual(25, "50 << -1");
        ExecutionTestUtils.AssertEqual(10, "5 >> -1");
    }
}
