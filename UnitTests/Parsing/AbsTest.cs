using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class AbsTest {


        [Test]
        public void TestAbsolute() {

            ParsingTestUtils.AssertEqual(
                new Function("abs",
                    new NumberLiteral("5")
                ),
                "|5|"
            );

        }


        [Test]
        public void TestNestedAbs() {
            ParsingTestUtils.AssertEqual(
                new Function("abs",
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add,
                        new Function("abs",
                            new NumberLiteral("2")
                        )
                    )
                ),
                "|(5 + |2|)|"
            );

            ParsingTestUtils.AssertEqual(
                new Function("abs",
                    new Function("sin",
                        new Function("abs", 
                            new NumberLiteral("5")    
                        )
                    )
                ),
                "|sin(|5|)|"
            );
        }


        [Test]
        public void TestImplicitMultiplication() {
            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("1"),
                    BinaryOperation.OperationType.Multiply,
                    new Function("abs",
                        new NumberLiteral("5")
                    )
                ),
                "1|5|"
            );

            ParsingTestUtils.AssertEqual(
                new BinaryOperation(
                    new Function("abs",
                        new NumberLiteral("5")
                    ),
                    BinaryOperation.OperationType.Multiply,
                    new NumberLiteral("1")
                ),
                "|5|1"
            );
        }


        [Test]
        public void TestFunctionArgument() {
            ParsingTestUtils.AssertEqual(
                new Function("sin",
                    new Function("abs",
                        new NumberLiteral("7")
                    )
                ),
                "sin |7|"
            );

            ParsingTestUtils.AssertEqual(
                new Function("sin",
                    new Function("abs",
                        new BinaryOperation(
                            new NumberLiteral("7"),
                            BinaryOperation.OperationType.Subtract,
                            new NumberLiteral("2")
                        )
                    )
                ),
                "sin |7-2|"
            );
        }


    }
}
