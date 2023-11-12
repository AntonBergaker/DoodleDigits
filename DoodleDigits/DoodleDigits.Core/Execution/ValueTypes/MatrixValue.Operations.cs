using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class MatrixValue {

    private static MatrixDimension DimensionFromArray(Value[,] values) {
        List<MatrixDimension> outerValues = new();
        List<MatrixValueElement> innerValues = new();

        for (int y = 0; y < values.GetLength(0); y++) {
            innerValues.Clear();
            for (int x = 0; x < values.GetLength(1); x++) {
                innerValues.Add(new MatrixValueElement(values[y, x]));
            }

            outerValues.Add(new MatrixDimension(innerValues));
        }

        return new MatrixDimension(outerValues);
    }

    private static MatrixDimension PerformOnAllElements(MatrixDimension matrix, Value other, BinaryOperation.OperationSide side, ExecutorContext context, BinaryNodes nodes, BinaryOperation.OperationFunction operation) {

        List<IMatrixElement> elements = new();
        foreach (IMatrixElement element in matrix) {
            if (element is MatrixDimension dim) {
                elements.Add(PerformOnAllElements(dim, other, side, context, nodes, operation));
            } else if (element is MatrixValueElement elem) {
                var value = side == BinaryOperation.OperationSide.Left ? 
                    operation(elem.Value, other, context, nodes) : 
                    operation(other, elem.Value, context, nodes);
                elements.Add(new MatrixValueElement(value));
            }
        }
        return new MatrixDimension(elements);
    }

    private Value? BinaryOperationPerElement(Value other, BinaryOperation.OperationSide side, ExecutorContext context,
        BinaryNodes nodes, BinaryOperation.OperationFunction function) {


        MatrixDimension PerformPerElementOperation(MatrixDimension lhs, MatrixDimension rhs) {
            List<IMatrixElement> elements = new();
            for (int i = 0; i < lhs.Length; i++) {
                var lhsElement = lhs[i];
                var rhsElement = rhs[i];

                if (lhsElement is MatrixDimension lhsDim && rhsElement is MatrixDimension rhsDim) {
                    elements.Add(PerformPerElementOperation(lhsDim, rhsDim));
                } else if (lhsElement is MatrixValueElement lhsElem && rhsElement is MatrixValueElement rhsElem) {
                    elements.Add(new MatrixValueElement(
                        function(lhsElem.Value, rhsElem.Value, context, nodes)
                    ));
                } else {
                    throw new Exception("Vectors were not in a proper state");
                }
            }
            return new MatrixDimension(elements);
        }


        if (other is MatrixValue otherMatrix) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrix, side);
            if (MatrixValue.SameDimensions(lhs, rhs) == false) {
                context.AddResult(new ResultError("Dimensions not same size", nodes.Operation.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            return new MatrixValue(PerformPerElementOperation(lhs.Dimension, rhs.Dimension), false);
        }

        return null;
    }

    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) =>
        BinaryOperationPerElement(other, side, context, nodes, BinaryOperations.Add);

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) =>
        BinaryOperationPerElement(other, side, context, nodes, BinaryOperations.Subtract);

    public override Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        if (other is MatrixValue otherMatrixValue) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrixValue, side);
            return MultiplyMatrices(lhs, rhs, context, nodes);
        }

        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var realOther)) {
            // Scalar matrix multiplication
            return new MatrixValue(PerformOnAllElements(this.Dimension, other, side, context, nodes, BinaryOperations.Multiply), false);
        }

        return null;
    }

    private static Value MultiplyMatrices(MatrixValue lhs, MatrixValue rhs, ExecutorContext context, BinaryNodes nodes) {


        if (lhs.DimensionCount <= 2 && rhs.DimensionCount <= 2) {

            // If the lhs is single dimension, make it stand up instead so the algo still works
            if (lhs.DimensionCount == 1) {

                lhs = new MatrixValue(
                    new MatrixDimension(new [] {new MatrixDimension((IEnumerable<IMatrixElement>)lhs.Dimension)}),
                    lhs.TriviallyAchieved);
            }
            
            if (lhs[0].Length != rhs.Dimension.Length) {
                context.AddResult(new ResultError($"Matrices dimension sizes do not match, {lhs[0].Length} != {rhs.Dimension.Length}", nodes.Operation.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            int commonDimension = rhs.Dimension.Length;

            Value[,] newMatrix = new Value[lhs.Dimension.Length, rhs[0].Length];
            for (int y = 0; y < newMatrix.GetLength(0); y++) {
                for (int x = 0; x < newMatrix.GetLength(1); x++) {
                    Value sum = BinaryOperations.Multiply(lhs[y][0], rhs[0][x], context, nodes);

                    for (int i = 1; i < commonDimension; i++) {
                        Value element = BinaryOperations.Multiply(lhs[y][i], rhs[i][x], context, nodes);
                        sum = BinaryOperations.Add(sum, element, context, nodes);
                    }

                    newMatrix[y, x] = sum;
                }
            }

            if (newMatrix.GetLength(0) == 1 && newMatrix.GetLength(1) == 1) {
                return newMatrix[0, 0];
            }

            // Turn 2d arrays with a dimension as 1 into proper 1d arrays
            if (newMatrix.GetLength(0) == 1) {
                List<MatrixValueElement> values = new();
                for (int i = 0; i < newMatrix.GetLength(1); i++) {
                    values.Add(new(newMatrix[0, i]));
                }
                return new MatrixValue(new MatrixDimension(values), false);
            }

            if (newMatrix.GetLength(1) == 1) {
                List<MatrixValueElement> values = new();
                for (int i = 0; i < newMatrix.GetLength(0); i++) {
                    values.Add(new(newMatrix[i, 0]));
                }
                return new MatrixValue(new MatrixDimension(values), false);
            }

            return new MatrixValue(DimensionFromArray(newMatrix), false);
        }

        return null!;
    }

    public override Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutorContext context, BinaryNodes nodes) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherReal)) {
                return new MatrixValue(PerformOnAllElements(this.Dimension, other, side, context, nodes, BinaryOperations.Divide), false);
            }
        }

        return null;
    }

    public static Value Cross(MatrixValue lhsMatrix, MatrixValue rhsMatrix, ExecutorContext context, BinaryNodes nodes) {
        if (lhsMatrix.DimensionCount != 1) {
            context.AddResult(new ResultError("Matrix must be a vector", nodes.Lhs.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }
        if (rhsMatrix.DimensionCount != 1) {
            context.AddResult(new ResultError("Matrix must be a vector", nodes.Rhs.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        // Runs a0*a1 - b0*b1
        MatrixValueElement DifferenceOfProducts(Value a0, Value a1, Value b0, Value b1) {
            return new MatrixValueElement(BinaryOperations.Subtract(
                BinaryOperations.Multiply(a0, a1, context, nodes),
                BinaryOperations.Multiply(b0, b1, context, nodes),
                context, nodes));
        }

        if (lhsMatrix.Dimension.Length == 3 && rhsMatrix.Dimension.Length == 3) {
            return new MatrixValue(new MatrixDimension(
                DifferenceOfProducts(lhsMatrix[1].Value, rhsMatrix[2], lhsMatrix[2], rhsMatrix[1]),
                DifferenceOfProducts(lhsMatrix[2].Value, rhsMatrix[0], lhsMatrix[0], rhsMatrix[2]),
                DifferenceOfProducts(lhsMatrix[0].Value, rhsMatrix[1], lhsMatrix[1], rhsMatrix[0])
            ));
        }

        context.AddResult(new ResultError("Cross product is only defined for 3 element vectors", nodes.Operation.Position));
        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }
}