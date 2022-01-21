using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Functions.Implementations.Named {
    static class VectorFunctions {
        [CalculatorFunction(FunctionExpectedType.Vector, "normalize", "normalise", "normal", "norm")]
        public static Value Normalize(Value value, ExecutionContext<Function> context) {
            if (value is MatrixValue matrix) {
                if (matrix.DimensionCount == 1) {
                    Rational total = matrix.Magnitude(context);
                    
                    if (total == Rational.Zero) {
                        return new MatrixValue(new MatrixValue.MatrixDimension(
                            Enumerable.Range(0, matrix.Dimension.Length).Select(_ => new MatrixValue.MatrixValueElement(new RealValue(0)))
                        ));
                    }

                    List<MatrixValue.MatrixValueElement> elements = new();
                    foreach (var val in matrix.Dimension) {
                        if (((val as MatrixValue.MatrixValueElement)?.Value) is IConvertibleToReal ctr) {
                            RealValue real = ctr.ConvertToReal(context);
                            elements.Add(new MatrixValue.MatrixValueElement(new RealValue(real.Value / total)));
                        }
                    }

                    return new MatrixValue(new MatrixValue.MatrixDimension(elements));
                }
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Vector, "magnitude")]
        public static Value Magnitude(Value value, ExecutionContext<Function> context) {
            if (value is MatrixValue matrix) {
                if (matrix.DimensionCount == 1) {
                    return new RealValue(matrix.Magnitude(context));
                }
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }

        [CalculatorFunction(FunctionExpectedType.Vector, "determinant", "det")]
        public static Value Determinant(Value value, ExecutionContext<Function> context) {
            if (value is MatrixValue matrix) {
                if (matrix.DimensionCount != 2 || matrix.Dimension.Length != ((MatrixValue.MatrixDimension)matrix.Dimension[0]).Length) {
                    context.AddResult(new ResultError("determinant only valid for square 2d matricies", context.Position));
                    return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
                }

                int size = matrix.Dimension.Length;
                Rational[,] startMatrix = new Rational[size, size];
                for (int x = 0; x < size; x++) {
                    for (int y = 0; y < size; y++) {
                        if (matrix[y][x].Value is RealValue realValue) {
                            startMatrix[y, x] = realValue.Value;
                        }
                        else {
                            AstNode errorNode = matrix[y][x].Value.SourceAstNode ?? context.Node;
                            context.AddResult(new ResultError("Non real value inside matrix", errorNode.Position));
                            return new UndefinedValue(UndefinedValue.UndefinedType.Error, errorNode);
                        }
                    }
                }

                Rational RecursiveDeterminant(Rational[,] rationalMatrix) {
                    int size = rationalMatrix.GetLength(0);
                    if (size == 2) {
                        return 
                            rationalMatrix[0, 0] * rationalMatrix[1, 1] -
                            rationalMatrix[0, 1] * rationalMatrix[1, 0];
                    }

                    Rational determinant = 0;
                    for (int i=0;i<size;i++) {
                        Rational newValue = rationalMatrix[0, i] * RecursiveDeterminant(Utils.RemoveRowAndColumnFromArray(rationalMatrix, 0, i));

                        if (i % 2 == 0) {
                            determinant += newValue;
                        } else {
                            determinant -= newValue;
                        }
                    }

                    return determinant;
                }

                return new RealValue(RecursiveDeterminant(startMatrix));
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error, context.Node);
        }
    }
}
