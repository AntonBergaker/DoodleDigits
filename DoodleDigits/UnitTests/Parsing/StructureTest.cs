using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing;
class StructureTest {

    [Test]
    public void TestNewlineSeparation() {

        ParsingTestUtils.AssertEqual(
            new NodeList(
                new () {
                    new NumberLiteral("5"),
                    new NumberLiteral("5")
                }
            ), "5\n5"
        );

        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new NumberLiteral("5"), 
                BinaryOperation.OperationType.Add, 
                new NumberLiteral("5")
            ), "5+\n5"
        );

        ParsingTestUtils.AssertEqual(
            new NumberLiteral("5"), 
        "5\n");



        ParsingTestUtils.AssertEqual(
            new NodeList(
                new() {
                    new NumberLiteral("5"),
                    new UnaryOperation(UnaryOperation.OperationType.Subtract, new NumberLiteral("5"))
                }
            ),
            "5\n-5");
    }

    [Test]
    public void TestUnfinishedBinary() {
        ParsingTestUtils.AssertEqual(
            new NumberLiteral("5"), 
        "5 + ");
    }
}
