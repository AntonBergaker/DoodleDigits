using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Parsing;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Ast {
    static class AstUtils {

        public static void AssertEqual(AstNode expected, string input) {
            Parser parser = new Parser(FunctionLibrary.Functions.SelectMany(x => x.Names));
            ParseResult result = parser.Parse(input);

            if (result.Root.Equals(expected) == false) {
                Assert.Fail($"Trees not equal.\nExpected: {expected}\nGot: {result.Root}\nFor input: {input}");
            }
        }

    }
}
