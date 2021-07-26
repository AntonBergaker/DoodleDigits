using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class EqualsTest {
        [Test]
        public void TestEqualsChain() {

            ParsingUtils.AssertEqual(new EqualsComparison.Builder(
                new NumberLiteral("5")) {
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") },
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") }
            }.Build(), "5 = 5 = 5");


            ParsingUtils.AssertEqual(new EqualsComparison.Builder(
                new NumberLiteral("5")) {
                    { EqualsComparison.EqualsSign.Equals, new NumberLiteral("5") },
                    { EqualsComparison.EqualsSign.NotEquals, new NumberLiteral("5") }
                }.Build(), "5 = 5 != 5");

        }

    }
}
