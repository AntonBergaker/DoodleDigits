using NUnit.Framework;
using System;
using System.Collections.Generic;
using DoodleDigits.Core;

namespace UnitTests {
    class TokenizerTest {

        [Test]
        public void TestBasicTokens() {

            AssertTokenEqual(new Token[] { 
                new("5", TokenType.Number), 
                new("+", TokenType.Add),
                new("5", TokenType.Number),
            } , "5 + 5");

        }

        [Test]
        public void TestIdentifier() {
            AssertTokenEqual(new Token[] {
                new("x", TokenType.Identifier),
                new("+", TokenType.Add),
                new("5", TokenType.Number),
            }, "x + 5");

            AssertTokenEqual(new Token[] {
                new("x", TokenType.Identifier),
                new("+", TokenType.Add),
                new("5", TokenType.Number),
            }, "x+5");

            AssertTokenEqual(new Token[] {
                new("epic_meme_variable_bro", TokenType.Identifier),
                new("+", TokenType.Add),
                new("5", TokenType.Number),
            }, "epic_meme_variable_bro+5");
        }


        [Test]
        public void TestNumber() {
            AssertTokenEqual(new Token[] {
                new("123", TokenType.Number),
            }, "123");

            AssertTokenEqual(new Token[] {
                new("123.52", TokenType.Number),
            }, "123.52");

            AssertTokenEqual(new Token[] {
                new("1_000", TokenType.Number),
            }, "1_000");

            AssertTokenEqual(new Token[] {
                new(".5050", TokenType.Number),
            }, ".5050");

            AssertTokenEqual(new Token[] {
                new("5 5", TokenType.Number),
                new("5", TokenType.Number),
            }, "5 5  5");
        }

        [Test]
        public void TestFunction() {
            AssertTokenEqual(new Token[] {
                new("func", TokenType.Identifier),
                new("(", TokenType.ParenthesisOpen),
                new("5", TokenType.Number),
                new(")", TokenType.ParenthesisClose),
            }, "func(5)");
        }

        [Test]
        public void TestManyCharacterTokens() {
            AssertTokenEqual(new Token[] {
                new("5", TokenType.Number),
                new("<=", TokenType.LessOrEqualTo),
                new("5", TokenType.Number),
                new("!=", TokenType.NotEquals),
                new("5", TokenType.Number),
                new("<", TokenType.LessThan),
                new("5", TokenType.Number),
            }, "5 <= 5 != 5 < 5");
        }

        [Test]
        public void TestSeperators() {

            AssertTokenEqual(new Token[] {
                new("5", TokenType.Number),
                new("\n", TokenType.NewLine),
                new("5", TokenType.Number),
            }, "5\n5");

        }


        private void AssertTokenEqual(Token[] expectedTokens, string input) {

            Tokenizer tokenizer = new Tokenizer();
            Token[] actualTokens = tokenizer.Tokenize(input);

            Assert.AreEqual(expectedTokens, actualTokens);
        }
    }
}
