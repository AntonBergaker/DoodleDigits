using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using NUnit.Framework;

namespace UnitTests.Execution;
class EqualsTest {
    [Test]
    public void TestBinaryEquals() {
        ExecutionTestUtils.AssertEqual(true, "5 = 5");
        ExecutionTestUtils.AssertEqual(false, "5 = 4");
        ExecutionTestUtils.AssertEqual(true, "x = 5, x = 5");
        ExecutionTestUtils.AssertEqual(false, "pi = 5");
        ExecutionTestUtils.AssertEqual(true, "a = pi*2, pi = a/2");

        ExecutionTestUtils.AssertEqual(5, "x = 5");
    }

    [Test]
    public void TestBinaryNotEquals() {
        ExecutionTestUtils.AssertEqual(false, "5 != 5");
        ExecutionTestUtils.AssertEqual(true, "5 != 4");

        ExecutionTestUtils.AssertEqual(true, "true != false");
    }

    [Test]
    public void TestManyEquals() {
        ExecutionTestUtils.AssertEqual(true, "5 = 5 = 5");
        ExecutionTestUtils.AssertEqual(true, "5 = 5 = 5 = 5 = 5");
        ExecutionTestUtils.AssertEqual(false, "5 = 5 = 5 = 5 = 6");
        ExecutionTestUtils.AssertEqual(false, "5 = 5 = 6 = 5 = 5");
    }

    [Test]
    public void TestSingleEquals() {
        {
            var result = ExecutionTestUtils.CalculateString("a = ");
            foreach (var item in result.Results) {
                if (item is ResultValue rv) {
                    if (rv.Value is not UndefinedValue) {
                        Assert.Fail($"Empty equals returned a value of {rv.Value}");
                    }
                }
            }
        }
    }
}
