using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Ast;
using NUnit.Framework;

namespace UnitTests.Ast {
    class FunctionsTest {

        [Test]
        public void TestSingleFunctions() {

            AstUtils.AssertEqual(
                new Function("sin", new[]
                {
                    new NumberLiteral("5")
                }), "sin(5)"
            );

            AstUtils.AssertEqual(
                new Function("sin", new[]
                {
                    new NumberLiteral("5")
                }), "sin 5"
            );

            AstUtils.AssertEqual(
                new Function("max", new[]
                {
                    new NumberLiteral("1"),
                    new NumberLiteral("2")
                }), "max(1, 2)"
            );
        }

        [Test]
        public void TestLogFunction() {
            
            AstUtils.AssertEqual(
                new Function("log", new []
                {
                    new NumberLiteral("5")
                }), "log(5)"
            );

            AstUtils.AssertEqual(
                new Function("log", new[]
                {
                    new NumberLiteral("5"),
                    new NumberLiteral("10"),
                }), "log10 5"
            );

            AstUtils.AssertEqual(
                new Function("log", new Expression[]
                {
                    new BinaryOperation(
                        new NumberLiteral("5"),
                        BinaryOperation.OperationType.Add, 
                        new NumberLiteral("5")
                        ),
                    new NumberLiteral("2"),
                }), "log_2(5+5)"
            );


            AstUtils.AssertEqual(
                new Function("log", new Expression[]
                {
                    new NumberLiteral("5"),
                    new Identifier("pi"),
                }), "log_pi(5)"
            );
        }
    }
}
