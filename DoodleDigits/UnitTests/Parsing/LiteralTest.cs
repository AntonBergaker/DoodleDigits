
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing {
    class LiteralTest {

        [Test]
        public void TestLiteralParsing() {
            ParsingTestUtils.AssertEqual(new NumberLiteral("123"), "123");
            ParsingTestUtils.AssertEqual(new NumberLiteral("123.123"), "123.123");
            ParsingTestUtils.AssertEqual(new NumberLiteral("0x10"), "0x10");
            ParsingTestUtils.AssertEqual(new NumberLiteral("0x10bcf"), "0x10bcf");
            ParsingTestUtils.AssertEqual(new NumberLiteral("0x10BCF"), "0x10BCF");
            ParsingTestUtils.AssertEqual(new NumberLiteral("0b10"), "0b10");
        }

        
    }
}
