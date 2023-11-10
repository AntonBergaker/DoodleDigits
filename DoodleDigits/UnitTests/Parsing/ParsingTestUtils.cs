using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Parsing;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing;
static class ParsingTestUtils {

    public static void AssertEqual(AstNode expected, string input) {
        AstNode result = ParseToAst(input);

        if (result.Equals(expected) == false) {
            Assert.Fail($"Trees not equal.\nExpected: {expected}\nGot: {result}\nFor input: {input}");
        }
    }

    public static AstNode ParseToAst(string input) {
        Parser parser = new Parser(FunctionLibrary.Functions);
        return parser.Parse(input).Root;
    }
}
