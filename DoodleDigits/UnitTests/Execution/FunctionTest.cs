using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.Execution; 
class FunctionTest {
    [Test]
    public void TestSimpleFunctions() {
        ExecutionTestUtils.AssertEqual( 2, "sqrt(4)" );
        ExecutionTestUtils.AssertEqual(2, "sqrt 4");
        ExecutionTestUtils.AssertEqual(4, "sqrt(4^2)");
    }

    [Test]
    public void TestComplexFunctions() {
        ExecutionTestUtils.AssertEqual(2, "log10 100");
        ExecutionTestUtils.AssertEqual(2, "log_10 100");
        ExecutionTestUtils.AssertEqual(2, "log(100, 10)");

        ExecutionTestUtils.AssertEqual(2, "root2 4");
        ExecutionTestUtils.AssertEqual(2, "root_2 4");
        ExecutionTestUtils.AssertEqual(2, "root(4, 2)");

    }

    [Test]
    public void TestVariableParameterCount() {
        ExecutionTestUtils.AssertEqual(5, "max(1, 2, 3, 4, 5)");
        ExecutionTestUtils.AssertEqual(1, "min(1, 2, 3, 4, 5)");

        ExecutionTestUtils.AssertEqual(15, "max(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)");
        ExecutionTestUtils.AssertEqual(1, "min(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)");
    }

    [Test]
    public void TestMax() {
        ExecutionTestUtils.AssertEqual(3, "max(1, 2, 3)");
    }

    [Test]
    public void TestMin() {
        ExecutionTestUtils.AssertEqual(1, "min(1, 2, 3, infinity)");
    }

    [Test]
    public void TestSum() {
        ExecutionTestUtils.AssertEqual(6, "sum(1, 2, 3)");
        ExecutionTestUtils.AssertEqual(1, "sum(1)");
        ExecutionTestUtils.AssertEqual(7, "sum( [1, 2, 4])");
    }

    [Test]
    public void TestMedian() {
        ExecutionTestUtils.AssertEqual(2, "median(1, 2, 3)");
        ExecutionTestUtils.AssertEqual(2, "median(1, 2, 4)");
        ExecutionTestUtils.AssertEqual(2, "median(1205, 1, 2)");

        ExecutionTestUtils.AssertEqual(2, "median(1, 3)");
        ExecutionTestUtils.AssertEqual(2, "median(-123, 1, 3, 5)");
    }
}
