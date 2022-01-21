using System;
using System.Collections.Generic;
using System.Numerics;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class MatrixValue {
    /// <summary>
    /// Recursive function that performs an operation on the entry pairs of two matrices 
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="context"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static MatrixDimension PerformPerElementOperation(MatrixDimension lhs, MatrixDimension rhs, ExecutionContext<BinaryOperation> context, BinaryOperation.OperationFunction operation) {
        List<IMatrixElement> elements = new();
        for (int i = 0; i < lhs.Length; i++) {
            var lhsElement = lhs[i];
            var rhsElement = rhs[i];

            if (lhsElement is MatrixDimension lhsDim && rhsElement is MatrixDimension rhsDim) {
                elements.Add(PerformPerElementOperation(lhsDim, rhsDim, context, operation));
            } else if (lhsElement is MatrixValueElement lhsElem && rhsElement is MatrixValueElement rhsElem) {
                elements.Add(new MatrixValueElement(
                    operation(lhsElem.Value, rhsElem.Value, context)
                ));
            } else {
                throw new Exception("Vectors were not in a proper state");
            }
        }
        return new MatrixDimension(elements);
    }

    private static MatrixDimension PerformOnAllElements(MatrixDimension matrix, Value other, BinaryOperation.OperationSide side, ExecutionContext<BinaryOperation> context, BinaryOperation.OperationFunction operation) {

        List<IMatrixElement> elements = new();
        for (int i = 0; i < matrix.Length; i++) {
            var element = matrix[i];

            if (element is MatrixDimension dim) {
                elements.Add(PerformOnAllElements(dim, other, side, context, operation));
            } else if (element is MatrixValueElement elem) {
                Value value;
                if (side == BinaryOperation.OperationSide.Left) {
                    value = operation(elem.Value, other, context);
                } else {
                    value = operation(other, elem.Value, context);
                }
                elements.Add(new MatrixValueElement(value));
            }
        }
        return new MatrixDimension(elements);
    }

    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrix) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrix, side);
            if (MatrixValue.SameDimensions(lhs, rhs) == false) {
                context.AddResult(new ResultError("Dimensions not same size", context.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            return new MatrixValue(PerformPerElementOperation(lhs.Dimension, rhs.Dimension, context, BinaryOperations.Add), false, context.Node);
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrix) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrix, side);
            if (MatrixValue.SameDimensions(lhs, rhs) == false) {
                context.AddResult(new ResultError("Dimensions not same size", context.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
            }

            return new MatrixValue(PerformPerElementOperation(lhs.Dimension, rhs.Dimension, context, BinaryOperations.Subtract), false, context.Node);
        }

        return null;
    }

    public override Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrixValue) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrixValue, side);
            var result = Multiply(lhs.Dimension, rhs.Dimension);
            if (result is MatrixDimension md) {
                return new MatrixValue(md, false, context.Node);
            }
            if (result is MatrixValueElement element) {
                return element.Value;
            }
        }

        else if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, out var realOther)) {
            // Scalar matrix multiplication
            return new MatrixValue(PerformOnAllElements(this.Dimension, other, side, context, BinaryOperations.Multiply), false, context.Node);
        }

        return null;
    }

    private static IMatrixElement Multiply(MatrixDimension lhs, MatrixDimension rhs) {
        return null!;
    }

    public override Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, out var otherReal)) {
                return new MatrixValue(PerformOnAllElements(this.Dimension, other, side, context, BinaryOperations.Divide), false, context.Node);
            }
        }

        return null;
    }
}