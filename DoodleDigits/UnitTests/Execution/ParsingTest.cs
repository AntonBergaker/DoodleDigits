using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using NUnit.Framework;
using Rationals;
using System.Numerics;

namespace UnitTests.Execution;
class ParsingTest {

    [Test]
    public void TestNumberParse() {
        ExecutionTestUtils.AssertEqual((Rational)5.123, "5.123");
        ExecutionTestUtils.AssertEqual(1_000_000, "1_000_000");

        string bigNumber = "100000000000000000000000";
        ExecutionTestUtils.AssertEqual(new Rational(BigInteger.Parse(bigNumber)), bigNumber);
    }

    [Test]
    public void TestConfusingComma() {
        var result = ExecutionTestUtils.CalculateString("5,2*5");

        var values = result.Results.OfType<ResultValue>().ToArray();
        Assert.AreEqual(2, values.Length);
        Assert.AreEqual(new RealValue(5), values[0].Value);
        Assert.AreEqual(ValueTriviality.Trivial, values[0].Value.Triviality);
        Assert.AreEqual(new RealValue(10), values[1].Value);
        Assert.AreEqual(ValueTriviality.NonTrivial, values[1].Value.Triviality);
    }
}
