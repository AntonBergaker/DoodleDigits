using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;
using NUnit.Framework;
using Rationals;

namespace UnitTests {
    class RationalTest {
        private static readonly (string @string, Rational rational)[] TestData = {
            ("1", 1),
            ("2", 2),
            ("10", 10),
            ("1000000", 1000000),
            ("-10", -10),
            ("-1000000", -1000000),
            ("0.0000001", (Rational)0.0000001M),
            ("-0.0000001", -(Rational)0.0000001M),
            ("0.00000012345", (Rational)0.00000012345M),
            ("-0.00000012345", -(Rational)0.00000012345M)
        };

        [Test]
        public void TestToDecimalString() {
            foreach (var (expected, rational) in TestData) {
                Assert.AreEqual(expected, rational.ToDecimalString(100));
            }
        }

        [Test]
        public void TestTryParse() {
            foreach (var (input, expected) in TestData) {
                if (RationalUtils.TryParse(input, out Rational actual, 10) == false) {
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
