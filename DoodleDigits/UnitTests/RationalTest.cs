using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;
using NUnit.Framework;
using Rationals;

namespace UnitTests {
    class RationalTest {
        private static readonly (string @string, Rational rational)[] DecimalTestData = {
            ("1", 1),
            ("2", 2),
            ("10", 10),
            ("1000000", 1000000),
            ("-10", -10),
            ("-1000000", -1000000),
            ("0.0000001", (Rational)0.0000001M),
            ("-0.0000001", -(Rational)0.0000001M),
            ("0.00000012345", (Rational)0.00000012345M),
            ("-0.00000012345", -(Rational)0.00000012345M),
        };

        [Test]
        public void TestToDecimalString() {
            foreach (var (expected, rational) in DecimalTestData) {
                Assert.AreEqual(expected, rational.ToDecimalString(100));
            }
        }

        [Test]
        public void TestTryParseDecimal() {
            foreach (var (input, expected) in DecimalTestData) {
                if (RationalUtils.TryParse(input, out Rational actual, maxMagnitude: 200, @base: 10) == false) {
                    Assert.Fail("Failed to parse " + input);
                }
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void TestToStringDecimalLimit() {
            Assert.AreEqual(
                "1234567891011121314151617181920",
                RationalUtils.ToDecimalString(
                    BigInteger.Parse("1234567891011121314151617181920"),
                    maximumDecimals: 1)
            );

            Assert.AreEqual(
                "1234567891011121314151617181920.2...",
                RationalUtils.ToDecimalString(
                    new Rational(
                        BigInteger.Parse("123456789101112131415161718192021"),
                        100),
                    maximumDecimals: 1)
            );

            Assert.AreEqual(
                "1234567891011121314151617181920.21...",
                RationalUtils.ToDecimalString(
                    new Rational(
                        BigInteger.Parse("1234567891011121314151617181920212223"),
                        1000000),
                    maximumDecimals: 2)
            );
        }

        private static readonly (string @string, Rational rational)[] ScientificTestData = {
            ("1E0", (Rational)1E0),
            ("2E0", (Rational)2E0),
            ("2E10", (Rational)2E10),
            ("1.123E5", (Rational)1.123E5),
            ("5E-1", (Rational)5E-1),
            ("-5.12E-12", (Rational)(-5.12E-12)),
        };

        [Test]
        public void TestToScientificString() {
            foreach (var (expected, rational) in ScientificTestData) {
                Assert.AreEqual(expected, rational.ToScientificString(100));
            }
        }

        [Test]
        public void TestTryParseScientific() {
            foreach (var (input, expected) in ScientificTestData) {
                if (RationalUtils.TryParse(input, out Rational actual, maxMagnitude: 200, @base: 10) == false) {
                    Assert.Fail("Failed to parse " + input);
                }
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void TestRound() {
            Assert.AreEqual(5, RationalUtils.Round(5));
            Assert.AreEqual(5, RationalUtils.Round((Rational) 5.3));
            Assert.AreEqual(6, RationalUtils.Round((Rational) 5.7));
            Assert.AreEqual(6, RationalUtils.Round(6));
            Assert.AreEqual(-5, RationalUtils.Round(-(Rational) 5.3));
            Assert.AreEqual(-6, RationalUtils.Round(-(Rational) 5.7));
        }

        [Test]
        public void TestCeil() {
            Assert.AreEqual(5, RationalUtils.Ceil(5));
            Assert.AreEqual(6, RationalUtils.Ceil((Rational) 5.3));
            Assert.AreEqual(-5, RationalUtils.Ceil(-(Rational) 5.3));
        }

        [Test]
        public void TestFloor() {
            Assert.AreEqual(5, RationalUtils.Floor(5));
            Assert.AreEqual(5, RationalUtils.Floor((Rational)5.3));
            Assert.AreEqual(5, RationalUtils.Floor((Rational)5.99));
            Assert.AreEqual(-6, RationalUtils.Floor(-(Rational)5.3));
        }

        [Test]
        public void TestRemainder() {
            Assert.AreEqual(0, RationalUtils.Modulus(10, 2));
            Assert.AreEqual(1, RationalUtils.Modulus(10, 3));

            Assert.AreEqual(1, RationalUtils.Modulus(6, (Rational)2.5));
            Assert.AreEqual((Rational)0.8, RationalUtils.Modulus(6, (Rational)2.6));

            Assert.AreEqual(2, RationalUtils.Modulus(-1, 3));
            Assert.AreEqual(1, RationalUtils.Modulus(-2, 3));
        }
    }
}
