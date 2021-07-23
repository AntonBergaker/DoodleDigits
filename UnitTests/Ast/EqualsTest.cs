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

            AstUtils.AssertEqual(new EqualsComparison.Builder(
                new NumberLiteral("5")) {
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") },
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") }
            }.Build(), "5 = 5 = 5");


            AstUtils.AssertEqual(new EqualsComparison.Builder(
                new NumberLiteral("5")) {
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") },
                    { EqualsComparison.EqualsSign.NotEquals, new NumberLiteral("5") }
                }.Build(), "5 = 5 != 5");

        }
    }
}
