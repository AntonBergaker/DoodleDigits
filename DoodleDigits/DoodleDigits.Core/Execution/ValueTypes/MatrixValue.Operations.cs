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
    /// <param name="operation"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static MatrixDimension PerformPerElementOperation(MatrixDimension lhs, MatrixDimension rhs, Func<RealValue, RealValue, RealValue> operation) {
        List<IMatrixElement> elements = new();
        for (int i = 0; i < lhs.Length; i++) {
            var lhsElement = lhs[i];
            var rhsElement = rhs[i];

            if (lhsElement is MatrixDimension lhsDim && rhsElement is MatrixDimension rhsDim) {
                elements.Add(PerformPerElementOperation(lhsDim, rhsDim, operation));
            } else if (lhsElement is MatrixValueElement lhsElem && rhsElement is MatrixValueElement rhsElem) {
                elements.Add(new MatrixValueElement(
                    operation(lhsElem.Value, rhsElem.Value)
                ));
            } else {
                throw new Exception("Vectors were not in a proper state");
            }
        }
        return new MatrixDimension(elements);
    }

    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrix) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrix, side);
            if (MatrixValue.SameDimensions(lhs, rhs) == false) {
                context.AddResult(new ResultError("Dimensions not same size", context.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            return new MatrixValue(PerformPerElementOperation(lhs.Dimension, rhs.Dimension, 
                (lhs, rhs) => lhs.Clone(value: lhs.Value + rhs.Value)
            ), false);
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrix) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrix, side);
            if (MatrixValue.SameDimensions(lhs, rhs) == false) {
                context.AddResult(new ResultError("Dimensions not same size", context.Position));
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            return new MatrixValue(PerformPerElementOperation(lhs.Dimension, rhs.Dimension,
                (lhs, rhs) => lhs.Clone(value: lhs.Value - rhs.Value)
            ), false);
        }

        return null;
    }

    public override Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (other is MatrixValue otherMatrixValue) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherMatrixValue, side);
            var result = Multiply(lhs.Dimension, rhs.Dimension);
            if (result is MatrixDimension md) {
                return new MatrixValue(md, false);
            }
            if (result is MatrixValueElement element) {
                return element.Value;
            }
        }

        else if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, out var realOther)) {
            // Scalar matrix multiplication
            return SelectAll(x => x.Clone(value: x.Value * realOther.Value));
        }

        return null;
    }

    private static IMatrixElement Multiply(MatrixDimension lhs, MatrixDimension rhs) {
        return null!;
    }

    public override Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
        if (side == BinaryOperation.OperationSide.Left) {
            if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, out var otherReal)) {
                if (otherReal.Value.IsZero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return SelectAll(x => x.Clone(value: x.Value / otherReal.Value));
            }
        }

        return null;
    }
}