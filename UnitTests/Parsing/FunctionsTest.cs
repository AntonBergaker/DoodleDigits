using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class FunctionsTest {

        [Test]
        public void TestSingleFunctions() {

            ParsingTestUtils.AssertEqual(
                new Function("sin", 
                    new NumberLiteral("5")
                ), "sin(5)"
            );

            ParsingTestUtils.AssertEqual(
                new Function("sin",
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add,
                        new NumberLiteral("5")
                    )
                ), "sin(5+5)"
            );

            ParsingTestUtils.AssertEqual(
                new Function("max",
                    new NumberLiteral("1"),
                    new NumberLiteral("2")
                ), "max(1, 2)"
            );
        }

        [Test]
        public void TestNonParenthesisParsing() {
            // sin 5 = sin(5)
            ParsingTestUtils.AssertEqual(
                new Function("sin",
                    new NumberLiteral("5")
                ),"sin 5"
            );

            // sin 5 + 5 = sin(5) + 5
            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new Function("sin",
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.Add,
                    new NumberLiteral("5")
                ), "sin 5 + 5"
            );

            // sin 5 * 5 = sin(5) * 5
            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new Function("sin",
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("5")
                ), "sin 5 * 5"
            );

            // sin 5 = 5 <=> sin(5) = 5
            ParsingTestUtils.AssertEqual(
                new EqualsComparison(
                    new Function("sin",
                        new NumberLiteral("5")
                    ),
                    EqualsComparison.EqualsSign.Equals,
                    new NumberLiteral("5")
                ), "sin 5 = 5"
            );

            // sin 2x = sin(2x)
            ParsingTestUtils.AssertEqual(
                new Function("sin",
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
                    new Function("sin",
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
                new Function("log", 
                    new NumberLiteral("5")
                ), "log(5)"
            );

            ParsingTestUtils.AssertEqual(
                new Function("log", 
                    new NumberLiteral("5"),
                    new NumberLiteral("10")
                ), "log10 5"
            );

            ParsingTestUtils.AssertEqual(
                new Function("log", 
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add, 
                        new NumberLiteral("5")
                    ),
                    new NumberLiteral("2")
                ), "log_2(5+5)"
            );


            ParsingTestUtils.AssertEqual(
                new Function("log", 
                    new NumberLiteral("5"),
                    new Identifier("pi")
                ), "log_pi(5)"
            );

            ParsingTestUtils.AssertEqual(
                new EqualsComparison(
                    new Function("log", 
                        new NumberLiteral("5"), 
                        new Identifier("e")
                    ),
                    EqualsComparison.EqualsSign.Equals,
                    new Function("ln", 
                        new NumberLiteral("5")
                    )
                ),
                "log_e(5) = ln(5)"
            );
        }
    }
}
