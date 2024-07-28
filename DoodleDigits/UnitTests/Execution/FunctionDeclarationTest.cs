using NUnit.Framework;

namespace UnitTests.Execution;
internal class FunctionDeclarationTest {
    [Test]
    public void SingleArgumentFunction() {
        ExecutionTestUtils.AssertEqual(2, "f(x) = x + 1\nf(1)");
    }

    [Test]
    public void TwoArgumentFunction() {
        ExecutionTestUtils.AssertEqual(15, "f(x, y) = x + y\nf(5, 10)");
    }
}
