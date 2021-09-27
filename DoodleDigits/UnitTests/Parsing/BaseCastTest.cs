using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class BaseCastTest {

        [Test]
        public void TestCastLiteral() {

            ParsingTestUtils.AssertEqual(
                new BaseCast(
                    new NumberLiteral("5"),
                    BaseCast.TargetType.Hex
                ), "5 as hex"
            );

            ParsingTestUtils.AssertEqual(
                new BaseCast(
                    new NumberLiteral("28.6"),
                    BaseCast.TargetType.Decimal
                ), "28.6 in decimal"
            );

            ParsingTestUtils.AssertEqual(
                new BaseCast(
                    new NumberLiteral("28.6"),
                    BaseCast.TargetType.Binary
                ), "28.6 as binary"
            );
        }

    }
}
