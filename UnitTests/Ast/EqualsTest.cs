using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using NUnit.Framework;

namespace UnitTests.Ast {
    class EqualsTest {
        [Test]
        public void TestEqualsChain() {

            AstUtils.AssertEqual(new EqualsChain(
                new[] { new NumberLiteral("5"), new NumberLiteral("5"), new NumberLiteral("5") },
                new[] { EqualsChain.EqualsType.Equals, EqualsChain.EqualsType.Equals }
            ), "5 = 5 = 5");


            AstUtils.AssertEqual(new EqualsChain(
                new[] { new NumberLiteral("5"), new NumberLiteral("5"), new NumberLiteral("5") },
                new[] { EqualsChain.EqualsType.Equals, EqualsChain.EqualsType.NotEquals }
            ), "5 = 5 != 5");

        }
    }
}
