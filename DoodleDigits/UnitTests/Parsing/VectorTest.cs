using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing;
class VectorTest {
    [Test]
    public void TestVectorDeclaration() {

        ParsingTestUtils.AssertEqual(
            new VectorDeclaration(
                new NumberLiteral("5"),
                new NumberLiteral("3")
            ), "(5, 3)"
        );

        ParsingTestUtils.AssertEqual(
            new VectorDeclaration(
                new BinaryOperation(
                    new NumberLiteral("1"),
                    BinaryOperation.OperationType.Add,
                    new NumberLiteral("2")
                ),
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("5")
                )
            ), "(1 + 2, 5(5) )"
        );

        ParsingTestUtils.AssertEqual(
            new VectorDeclaration(new[] {
                new VectorDeclaration(new [] {
                    new NumberLiteral("5"),
                    new NumberLiteral("3")
                }),
                new VectorDeclaration(new [] {
                    new NumberLiteral("5"),
                    new NumberLiteral("2")
                }),

            }), "( (5, 3), (5, 2) )"
        );

        ParsingTestUtils.AssertEqual(
            new VectorDeclaration(
                new VectorDeclaration(
                    new NumberLiteral("5"),
                    new NumberLiteral("3")
                ),
                new VectorDeclaration(
                    new NumberLiteral("5"),
                    new NumberLiteral("2")
                )
            ), "[ (5, 3), (5, 2) ]"
        );
    }

    [Test]
    public void TestFunctionWithVectorInput() {

        ParsingTestUtils.AssertEqual(
            new Function("normalize",
                new VectorDeclaration(
                    new NumberLiteral("5"),
                    new NumberLiteral("3")
                )
            ), "normalize (5, 3)"
        );

        ParsingTestUtils.AssertEqual(
            new Function("abs",
                new VectorDeclaration(
                    new NumberLiteral("25"),
                    new NumberLiteral("5")
                )
            ), "abs (25, 5)"
        );

    }

    [Test]
    public void TestVectorImplicitMultiplication() {

        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new VectorDeclaration(
                    new NumberLiteral("5"),
                    new NumberLiteral("5")
                ),
                BinaryOperation.OperationType.Multiply,
                new VectorDeclaration(
                    new NumberLiteral("1"),
                    new NumberLiteral("2")
                )
            ), "(5, 5) (1, 2)"
        );

        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new VectorDeclaration(
                    new NumberLiteral("7"),
                    new NumberLiteral("8")
                ),
                BinaryOperation.OperationType.Multiply,
                new VectorDeclaration(
                    new NumberLiteral("1"),
                    new NumberLiteral("2")
                )
            ), "[7, 8] [1, 2]"
        );
    }
}
