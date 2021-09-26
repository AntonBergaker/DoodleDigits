using System;
using DoodleDigits.Core.Execution.ValueTypes;
using NUnit.Framework;

namespace UnitTests.Execution {
    class PresentationTest {
        private void AssertEqualPresentation(RealValue.PresentedForm expected, string input) {
            var result = ExecutionTestUtils.CalculateValueString(input);

            if (result is not RealValue rv) {
                Assert.Fail("Result is not a real value");
                throw new Exception();
            }

            Assert.AreEqual(expected, rv.Form);
        }

        [Test]
        public void TestLiterals() {
            AssertEqualPresentation(RealValue.PresentedForm.Hex,"0x1");
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "0b1");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "12");
        }

        [Test]
        public void TestUnaryOperations() {
            AssertEqualPresentation(RealValue.PresentedForm.Hex, "-0x1");
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "0b1");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "12!");
        }

        [Test]
        public void TestBinaryOperations() {
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "0b1 + 0b0");
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "0b1 - 0b11");
            AssertEqualPresentation(RealValue.PresentedForm.Hex, "0x1 + 0x2");
            AssertEqualPresentation(RealValue.PresentedForm.Hex, "0x1 * 0x2");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "10 bxor 10");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "10 band -10");
        }

        [Test]
        public void TestNamedFunctions() {
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "sin(0b0)");
            AssertEqualPresentation(RealValue.PresentedForm.Binary, "sqrt(0b0.001");
            AssertEqualPresentation(RealValue.PresentedForm.Hex, "tan(0x12)");
            AssertEqualPresentation(RealValue.PresentedForm.Hex, "log(0x54)");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "tanh(10)");
            AssertEqualPresentation(RealValue.PresentedForm.Decimal, "cosh(10)");
        }

    }
}
