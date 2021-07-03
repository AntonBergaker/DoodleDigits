using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Ast;
using NUnit.Framework;

namespace UnitTests.Ast {
    class UnaryTest {

        [Test]
        public void TestUnary() {

            AstUtils.AssertEqual(new UnaryOperation(UnaryOperation.OperationType.Subtract, new NumberLiteral("5")), "-5");

            AstUtils.AssertEqual(
                new UnaryOperation(UnaryOperation.OperationType.Subtract,
                new UnaryOperation(UnaryOperation.OperationType.Subtract, 
                    new NumberLiteral("5")
                    )
                ), "-(-5)");

        }
    }
}
