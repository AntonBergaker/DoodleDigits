using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class BinaryOperationTest {

        [Test]
        public void TestBasicSingles() {

            ParsingUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("5"), 
                BinaryOperation.OperationType.Add,
                new NumberLiteral("5")
                ), "5 + 5");


            ParsingUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("5"),
                BinaryOperation.OperationType.Multiply,
                new NumberLiteral("5.125")
            ), "5 * 5.125");

            ParsingUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("12345"),
                BinaryOperation.OperationType.Subtract,
                new NumberLiteral("5.125")
            ), "12345-5.125");
        }

        [Test]
        public void TestArithmeticOrderOfOperations() {

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("1"),
                        BinaryOperation.OperationType.Multiply,
                        new NumberLiteral("2")
                        ),
                    BinaryOperation.OperationType.Subtract,
                    new NumberLiteral("3")
                    )
            , "1*2-3");

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("4"),
                    BinaryOperation.OperationType.Subtract,
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new NumberLiteral("6")
                    )
                )
                , "4-5*6");


            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("10"),
                        BinaryOperation.OperationType.Subtract,
                        new NumberLiteral("1")
                    ),
                    BinaryOperation.OperationType.Subtract,
                    new NumberLiteral("2")
                )
                , "10-1-2");
        }


        [Test]
        public void TestBooleanOrderOfOperations() {


            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new EqualsComparison(
                        new NumberLiteral("5"),
                        EqualsComparison.EqualsSign.Equals,
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.BooleanAnd,
                    new EqualsComparison(
                        new NumberLiteral("5"),
                        EqualsComparison.EqualsSign.NotEquals,
                        new NumberLiteral("5")
                    )
                )
                , "5 = 5 && 5 != 5");

            
            // This is becoming quite unreadable
            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new EqualsComparison(
                        new NumberLiteral("5"),
                        EqualsComparison.EqualsSign.Equals,
                        new BinaryOperation(
                            new NumberLiteral("5"),
                            BinaryOperation.OperationType.Add,
                            new NumberLiteral("5")
                        )
                    ),
                    BinaryOperation.OperationType.BooleanAnd,
                    new EqualsComparison(
                        new BinaryOperation(
                            new NumberLiteral("5"),
                            BinaryOperation.OperationType.Add,
                            new NumberLiteral("5")
                        ),
                        EqualsComparison.EqualsSign.NotEquals,
                        new NumberLiteral("5")
                    )
                )
                , "5 = 5 + 5 && 5 + 5 != 5");

        }

        [Test]
        public void TestImplicitMultiplication() {
            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add,
                        new NumberLiteral("5")
                    ), 
                    BinaryOperation.OperationType.Multiply,
                        new NumberLiteral("5")
                    ), "(5 + 5)(5)"
                );

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add,
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("5")
                ), "(5 + 5)5"
            );

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Add,
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new BinaryOperation(
                            new NumberLiteral("5"),
                            BinaryOperation.OperationType.Add,
                            new NumberLiteral("5")
                        )
                    )
                ) , "5+5(5 + 5)"
            );


            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Multiply,
                    new Identifier("x")
                ), "5x"
            );

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new Identifier("x")
                    ), 
                    BinaryOperation.OperationType.Multiply, 
                    new Identifier("y")
                ), "5x(y)"
            );


            ParsingUtils.AssertEqual(
                new EqualsComparison(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new NumberLiteral("5")
                    ),
                    EqualsComparison.EqualsSign.Equals,
                    new NumberLiteral("5")
                ),
                "5(5) = 5"
            );


            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Multiply,
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Power,
                        new NumberLiteral("5")
                    )
                ),
                "(5)(5)^5"
            );

            ParsingUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Power,
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("5")
                ),
                "5^5(5)"
            );
        }
    }
}
