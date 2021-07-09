using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using NUnit.Framework;

namespace UnitTests.Ast {
    class StructureTest {

        [Test]
        public void TestNewlineSeparation() {

            AstUtils.AssertEqual(
                new ExpressionList(
                    new () {
                        new NumberLiteral("5"),
                        new NumberLiteral("5")
                    }
                ), "5\n5"
            );

            AstUtils.AssertEqual(
                new BinaryOperation(
                    new NumberLiteral("5"), 
                    BinaryOperation.OperationType.Add, 
                    new NumberLiteral("5")
                ), "5+\n5"
            );

        }
    }
}
