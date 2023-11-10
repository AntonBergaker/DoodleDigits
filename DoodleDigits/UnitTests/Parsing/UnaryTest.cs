using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing; 
class UnaryTest {

    [Test]
    public void TestPreUnary() {

        ParsingTestUtils.AssertEqual(new UnaryOperation(UnaryOperation.OperationType.Subtract, new NumberLiteral("5")), "-5");

        ParsingTestUtils.AssertEqual(
            new UnaryOperation(UnaryOperation.OperationType.Subtract,
            new UnaryOperation(UnaryOperation.OperationType.Subtract, 
                new NumberLiteral("5")
                )
            ), "-(-5)");

    }

    [Test]
    public void TestPostUnary() {

        ParsingTestUtils.AssertEqual(
            new UnaryOperation(UnaryOperation.OperationType.Factorial, 
                new NumberLiteral("1")
            ),
            "1!");

        ParsingTestUtils.AssertEqual(
            new UnaryOperation(UnaryOperation.OperationType.Factorial,
                new UnaryOperation(UnaryOperation.OperationType.Factorial,
                    new NumberLiteral("1")
                )
            ),
            "1!!");

        ParsingTestUtils.AssertEqual(
            new UnaryOperation(UnaryOperation.OperationType.Subtract,
                new UnaryOperation(UnaryOperation.OperationType.Factorial,
                    new NumberLiteral("1")
                )
            ),
            "-1!");
    }
}
