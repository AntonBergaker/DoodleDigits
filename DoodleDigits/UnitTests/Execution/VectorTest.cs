using DoodleDigits.Core.Execution.ValueTypes;
using NUnit.Framework;

namespace UnitTests.Execution;
class VectorTest {
    [Test]
    public void TestValidate() {
        { // A 1d matrix with 2 elements is valid
            MatrixValue value = new MatrixValue(
                new MatrixValue.MatrixDimension(
                    new MatrixValue.MatrixValueElement(new RealValue(1)),
                    new MatrixValue.MatrixValueElement(new RealValue(2))
                )
            );

            Assert.IsTrue(value.IsValid);
        }

        { // A 2d matrix where one dimension is actually not a value is invalid
            MatrixValue value = new MatrixValue(
                new MatrixValue.MatrixDimension(
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(1)),
                        new MatrixValue.MatrixValueElement(new RealValue(2))
                    ),
                    new MatrixValue.MatrixValueElement(new RealValue(2))
                )
            );

            Assert.IsFalse(value.IsValid);
        }

        { // A 2x2 matrix is valid
            MatrixValue value = new MatrixValue(
                new MatrixValue.MatrixDimension(
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(1)),
                        new MatrixValue.MatrixValueElement(new RealValue(2))
                    ),
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(3)),
                        new MatrixValue.MatrixValueElement(new RealValue(4))
                    )
                )
            );

            Assert.IsTrue(value.IsValid);
        }

        { // A 2x2 matrix where one entry is actually 2x3 is invalid
            MatrixValue value = new MatrixValue(
                new MatrixValue.MatrixDimension(
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(1)),
                        new MatrixValue.MatrixValueElement(new RealValue(2))
                    ),
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(3)),
                        new MatrixValue.MatrixValueElement(new RealValue(4)),
                        new MatrixValue.MatrixValueElement(new RealValue(5))
                    )
                )
            );

            Assert.IsFalse(value.IsValid);
        }
    }

    [Test]
    public void TestConstructor() {
        {
            MatrixValue value = new MatrixValue(
                new MatrixValue.MatrixDimension(
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(1)),
                        new MatrixValue.MatrixValueElement(new RealValue(2))
                    ),
                    new MatrixValue.MatrixDimension(
                        new MatrixValue.MatrixValueElement(new RealValue(3)),
                        new MatrixValue.MatrixValueElement(new RealValue(4))
                    )
                )
            );

            Assert.AreEqual(value[0][0].Value, new RealValue(1));
            Assert.AreEqual(value[0][1].Value, new RealValue(2));
            Assert.AreEqual(value[1][0].Value, new RealValue(3));
            Assert.AreEqual(value[1][1].Value, new RealValue(4));
        }

        {
            MatrixValue value = (MatrixValue)ExecutionTestUtils.CalculateValueString("[[1, 2], [3, 4]]");
            Assert.AreEqual(value[0][0].Value, new RealValue(1));
            Assert.AreEqual(value[0][1].Value, new RealValue(2));
            Assert.AreEqual(value[1][0].Value, new RealValue(3));
            Assert.AreEqual(value[1][1].Value, new RealValue(4));
        }
    }

    [Test]
    public void TestVariableInstantiation() {

        ExecutionTestUtils.AssertEqual("[[1, 2], [3, 4]]", " a = [1, 2], b = [3, 4], [a, b] " );

    }

    [Test]
    public void TestMultiplication() {
        ExecutionTestUtils.AssertEqual(
            11,
            "[1, 2] * [3, 4]"
        );

        ExecutionTestUtils.AssertEqual(
            "[5, 8]",
            "[1, 2] * [[1,2],  [2,3]]"
        );

        ExecutionTestUtils.AssertEqual(
            "[[19, 22], [43, 50]]",
            "[[1, 2], [3, 4]] * [[5, 6], [7, 8]]"
        );

    }

    [Test]
    public void TestScalar() {
        ExecutionTestUtils.AssertEqual(
            "[2, 4]",
            "[1, 2] * 2"
        );

        ExecutionTestUtils.AssertEqual(
            "[2, 4]",
            "2 * [1, 2]"
        );

        ExecutionTestUtils.AssertEqual(
            "[1, 2]",
            "[2, 4] / 2"
        );
    }

    [Test]
    public void TestAddition() {
        ExecutionTestUtils.AssertEqual(
            "[12, 24]",
            "[6, 11] + [6, 13]"
        );

        ExecutionTestUtils.AssertEqual(
            "[6, 8, 10, 12]",
            "[1, 2, 3, 4] + [5, 6, 7, 8]"
        );

        ExecutionTestUtils.AssertEqual(
            "[[6, 8], [10, 12]]",
            "[[1, 2], [3, 4]] + [[5, 6], [7, 8]]"
        );
    }


    [Test]
   public void TestMagnitude() {
        ExecutionTestUtils.AssertEqual(1, "magnitude (1, 0)");
        ExecutionTestUtils.AssertEqual(10, "magnitude (6, 8)");
        ExecutionTestUtils.AssertEqual(13, "magnitude (12, 3, 4)");
   }

   [Test]
   public void TestDeterminant() {

       ExecutionTestUtils.AssertEqual(1, "det [[1, 0], [0, 1]]");
       ExecutionTestUtils.AssertEqual(-2, "det [[1, 2], [3, 4]]");
       ExecutionTestUtils.AssertEqual(0, "det [[1, 2, 3], [4, 5, 6], [7, 8, 9]]");
       ExecutionTestUtils.AssertEqual(-9, "det [[1, 2, 3], [4, 5, 6], [7, 8, 12]]");
    }
}
