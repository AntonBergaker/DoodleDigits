using NUnit.Framework;
using Rationals;

namespace UnitTests.Execution;
class LiteralTest {

    [Test]
    public void TestLiterals() {
        ExecutionTestUtils.AssertEqual(123, "123");
        ExecutionTestUtils.AssertEqual((Rational)123.123, "123.123");
        ExecutionTestUtils.AssertEqual(0x10, "0x10");
        ExecutionTestUtils.AssertEqual(0x10bcf, "0x10bcf");
        ExecutionTestUtils.AssertEqual(0x10BCF, "0x10BCF");
        ExecutionTestUtils.AssertEqual(0b10, "0b10");
    }
}
