using DoodleDigits.Core.Execution.Results;
using NUnit.Framework;

namespace UnitTests.Execution;
class VariableTest {
    [Test]
    public void TestVariableAssignment() {

        ExecutionTestUtils.AssertEqual(3, "x = 3, x");

        ExecutionTestUtils.AssertEqual(6, "x = 1, y = x + 5, y");

    }

    [Test]
    public void TestTriviallyAchievedAssignment() {
        {
            var result = ExecutionTestUtils.CalculateString("x = true").Results.LastOrDefault() as ResultValue;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.TriviallyAchieved);
        }

        {
            var result = ExecutionTestUtils.CalculateString("x = 5").Results.LastOrDefault() as ResultValue;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.TriviallyAchieved);
        }
    }
}
