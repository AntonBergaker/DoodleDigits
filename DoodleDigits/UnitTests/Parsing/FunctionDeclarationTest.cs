using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;
using NUnit.Framework;

namespace UnitTests.Parsing;
internal class FunctionDeclarationTest {
    [Test]
    public void OneParameterDeclaration() {

        ParsingTestUtils.AssertEqual(
            new FunctionDeclarationOrEquals(new(
                "f",
                ["x"],
                new BinaryOperation(
                    new Identifier("x"),
                    BinaryOperation.OperationType.Add,
                    new NumberLiteral("1")
                )
            )), "f(x) = x+1"
        );
    }

    public void TwoVariableDeclaration() {

        ParsingTestUtils.AssertEqual(
            new FunctionDeclarationOrEquals(new(
                "f",
                ["x, y"],
                new BinaryOperation(
                    new Identifier("x"),
                    BinaryOperation.OperationType.Add,
                    new Identifier("y")
                )
            )), "f(x, y) = x+y"
        );
    }
}
