﻿using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing;
class FunctionCallTest {

    [Test]
    public void TestSingleFunctions() {

        ParsingTestUtils.AssertEqual(
            new FunctionCall("sin", 
                new NumberLiteral("5")
            ), "sin(5)"
        );

        ParsingTestUtils.AssertEqual(
            new FunctionCall("sin",
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Add,
                    new NumberLiteral("5")
                )
            ), "sin(5+5)"
        );

        ParsingTestUtils.AssertEqual(
            new FunctionCall("max",
                new NumberLiteral("1"),
                new NumberLiteral("2")
            ), "max(1, 2)"
        );
    }

    [Test]
    public void TestNonParenthesisParsing() {
        // sin 5 = sin(5)
        ParsingTestUtils.AssertEqual(
            new FunctionCall("sin",
                new NumberLiteral("5")
            ),"sin 5"
        );

        // sin 5 + 5 = sin(5) + 5
        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new FunctionCall("sin",
                    new NumberLiteral("5")
                ),
                BinaryOperation.OperationType.Add,
                new NumberLiteral("5")
            ), "sin 5 + 5"
        );

        // sin 5 * 5 = sin(5) * 5
        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new FunctionCall("sin",
                    new NumberLiteral("5")
                ),
                BinaryOperation.OperationType.Multiply,
                new NumberLiteral("5")
            ), "sin 5 * 5"
        );

        // sin 5 = 5 <=> sin(5) = 5
        ParsingTestUtils.AssertEqual(
            new Comparison(
                new FunctionCall("sin",
                    new NumberLiteral("5")
                ),
                Comparison.ComparisonType.Equals,
                new NumberLiteral("5")
            ), "sin 5 = 5"
        );

        // sin 2x = sin(2x)
        ParsingTestUtils.AssertEqual(
            new FunctionCall("sin",
                new BinaryOperation(
                    new NumberLiteral("2"),
                    BinaryOperation.OperationType.Multiply,
                    new Identifier("x")
                )
            ), "sin 2x"
        );

        // sin 6x + x = sin(6x) + x
        ParsingTestUtils.AssertEqual(
            new BinaryOperation(
                new FunctionCall("sin",
                    new BinaryOperation(
                        new NumberLiteral("6"),
                        BinaryOperation.OperationType.Multiply,
                        new Identifier("x")
                    )
                ),
                BinaryOperation.OperationType.Add,
                new Identifier("x")
            ), "sin 6x + x"
        );
    }

    [Test]
    public void TestInlineParameterFunction() {
        
        ParsingTestUtils.AssertEqual(
            new FunctionCall("log", 
                new NumberLiteral("5")
            ), "log(5)"
        );

        ParsingTestUtils.AssertEqual(
            new FunctionCall("log", 
                new NumberLiteral("5"),
                new NumberLiteral("10")
            ), "log10 5"
        );

        ParsingTestUtils.AssertEqual(
            new FunctionCall("log", 
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Add, 
                    new NumberLiteral("5")
                ),
                new NumberLiteral("2")
            ), "log_2(5+5)"
        );


        ParsingTestUtils.AssertEqual(
            new FunctionCall("log", 
                new NumberLiteral("5"),
                new Identifier("pi")
            ), "log_pi(5)"
        );

        ParsingTestUtils.AssertEqual(
            new Comparison(
                new FunctionCall("log", 
                    new NumberLiteral("5"), 
                    new Identifier("e")
                ),
                Comparison.ComparisonType.Equals,
                new FunctionCall("ln", 
                    new NumberLiteral("5")
                )
            ),
            "log_e(5) = ln(5)"
        );
    }

    [Test]
    public void TestNewLineOnCustomFunctions() {
        ParsingTestUtils.AssertEqual(
            new NodeList([
                new Identifier("test"),
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Add,
                    new NumberLiteral("5")
                )
            ]), "test\n(5) + 5"
        );
    }
}
