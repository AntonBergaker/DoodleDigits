using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing;
class ComparisonTest {
    [Test]
    public void TestEqualsChain() {

        ParsingTestUtils.AssertEqual(new Comparison.Builder(
            new NumberLiteral("5")) {
                { Comparison.ComparisonType.Equals, new NumberLiteral("5") },
                { Comparison.ComparisonType.Equals, new NumberLiteral("5") }
        }.Build(), "5 = 5 = 5");


        ParsingTestUtils.AssertEqual(new Comparison.Builder(
            new NumberLiteral("5")) {
                { Comparison.ComparisonType.Equals, new NumberLiteral("5") },
                { Comparison.ComparisonType.NotEquals, new NumberLiteral("5") }
            }.Build(), "5 = 5 != 5");

    }

    [Test]
    public void TestComparisonChain() {

        ParsingTestUtils.AssertEqual(new Comparison.Builder(
            new NumberLiteral("5")) {
            { Comparison.ComparisonType.LessThan, new NumberLiteral("4") },
            { Comparison.ComparisonType.GreaterThan, new NumberLiteral("5") }
        }.Build(), "5 < 4 > 5");


        ParsingTestUtils.AssertEqual(new Comparison.Builder(
            new NumberLiteral("5")) {
            { Comparison.ComparisonType.Equals, new NumberLiteral("5") },
            { Comparison.ComparisonType.NotEquals, new NumberLiteral("5") },
            {Comparison.ComparisonType.LessThan, new NumberLiteral("2")}
        }.Build(), "5 = 5 != 5 < 2");

    }
}
