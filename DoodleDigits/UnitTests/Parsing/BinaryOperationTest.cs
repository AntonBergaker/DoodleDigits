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

            ParsingTestUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("5"), 
                BinaryOperation.OperationType.Add,
                new NumberLiteral("5")
                ), "5 + 5");


            ParsingTestUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("5"),
                BinaryOperation.OperationType.Multiply,
                new NumberLiteral("5.125")
            ), "5 * 5.125");

            ParsingTestUtils.AssertEqual(new BinaryOperation(
                new NumberLiteral("12345"),
                BinaryOperation.OperationType.Subtract,
                new NumberLiteral("5.125")
            ), "12345-5.125");
        }

        [Test]
        public void TestArithmeticOrderOfOperations() {

            ParsingTestUtils.AssertEqual(
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

            ParsingTestUtils.AssertEqual(
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


            ParsingTestUtils.AssertEqual(
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


            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new Comparison(
                        new NumberLiteral("5"),
                        Comparison.ComparisonType.Equals,
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.BooleanAnd,
                    new Comparison(
                        new NumberLiteral("5"),
                        Comparison.ComparisonType.NotEquals,
                        new NumberLiteral("5")
                    )
                )
                , "5 = 5 && 5 != 5");

            
            // This is becoming quite unreadable
            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new Comparison(
                        new NumberLiteral("5"),
                        Comparison.ComparisonType.Equals,
                        new BinaryOperation(
                            new NumberLiteral("5"),
                            BinaryOperation.OperationType.Add,
                            new NumberLiteral("5")
                        )
                    ),
                    BinaryOperation.OperationType.BooleanAnd,
                    new Comparison(
                        new BinaryOperation(
                            new NumberLiteral("5"),
                            BinaryOperation.OperationType.Add,
                            new NumberLiteral("5")
                        ),
                        Comparison.ComparisonType.NotEquals,
                        new NumberLiteral("5")
                    )
                )
                , "5 = 5 + 5 && 5 + 5 != 5");

        }

        [Test]
        public void TestImplicitMultiplication() {
            ParsingTestUtils.AssertEqual(
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

            ParsingTestUtils.AssertEqual(
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

            ParsingTestUtils.AssertEqual(
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


            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.Multiply,
                    new Identifier("x")
                ), "5x"
            );

            ParsingTestUtils.AssertEqual(
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


            ParsingTestUtils.AssertEqual(
                new Comparison(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new NumberLiteral("5")
                    ),
                    Comparison.ComparisonType.Equals,
                    new NumberLiteral("5")
                ),
                "5(5) = 5"
            );


            ParsingTestUtils.AssertEqual(
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

            ParsingTestUtils.AssertEqual(
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

            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Multiply,
                        new Identifier("a")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("5")
                ), "5a(5)"
            );
        }


        [Test]
        public void TestBitwiseOperations() {


            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.BitwiseXor,
                        new NumberLiteral("1")

                ), "5 bxor 1"
            );

            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.BitwiseAnd,
                    new NumberLiteral("1")

                ), "5 band 1"
            );

            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"),
                    BinaryOperation.OperationType.BitwiseXor,
                    new NumberLiteral("1")

                ), "5 bxor 1"
            );
        }

        [Test]
        public void TestImplicitMultiplicationPriority() {
            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("1"),
                        BinaryOperation.OperationType.Divide,
                        new NumberLiteral("2")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new Identifier("x")
                ), "1/2x"
            );

            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Divide,
                        new NumberLiteral("2")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("1")
                ), "5/2(1)"
            );
        }

    }
}
