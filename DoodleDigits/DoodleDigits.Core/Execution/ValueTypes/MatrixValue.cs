using DoodleDigits.Core.Utilities;
using Rationals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes; 
public partial class MatrixValue : Value {
    public interface IMatrixElement { }

    public class MatrixDimension : IMatrixElement, IEnumerable<IMatrixElement> {
        private readonly IMatrixElement[] _elements;
        public readonly int Length;

        public IMatrixElement this[int index] => _elements[index];

        public MatrixDimension(IEnumerable<IMatrixElement> elements) {
            this._elements = elements.ToArray();
            Length = this._elements.Length;
        }

        public MatrixDimension(params IMatrixElement[] elements) : this((IEnumerable<IMatrixElement>)elements) { }

        public IEnumerator<IMatrixElement> GetEnumerator() {
            return ((IEnumerable<IMatrixElement>)_elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override bool Equals(object? obj) {
            if (obj is not MatrixDimension otherDimension) {
                return false;
            }

            return Enumerable.SequenceEqual(_elements, otherDimension._elements);
        }

        public override int GetHashCode() {
            return HashCode.Combine(_elements);
        }
    }

    public class MatrixValueElement : IMatrixElement {
        public readonly Value Value;

        public MatrixValueElement(Value value) {
            Value = value;
        }

        public override bool Equals(object? obj) {
            if (obj is not MatrixValueElement otherValue) { 
                return false;
            }

            return otherValue.Value.Equals(Value);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }

    /// <summary>
    /// Helper struct with implicit casts that are sometimes weird but make the matrix easier to work with.
    /// </summary>
    public struct MatrixElement {
        private IMatrixElement _element;

        public MatrixElement this[int index] {
            get {
                if (_element is MatrixDimension md) {
                    return new MatrixElement(md[index]);
                }

                if (index == 0) {
                    return this;
                }

                throw new InvalidOperationException("Invalid attempt to reach depth of matrix. Check matrix dimensions.");
            }
        }

        public int Length { 
            get {
                if (_element is MatrixDimension md) {
                    return md.Length;
                }

                // Inside a value the width is the 1
                return 1;
            }
        }

        public static implicit operator Value(MatrixElement matrixElement) => matrixElement.Value;

        public Value Value {
            get {
                if (_element is MatrixValueElement mve) {
                    return mve.Value;
                }

                throw new InvalidOperationException("Attempted to access value before reaching bottom of matrix. Check matrix dimensions.");
            }
        }

        public MatrixElement(IMatrixElement element) {
            this._element = element;
        }
    }

    public MatrixElement this[int index] => new MatrixElement(Dimension[index]);

    public int DimensionCount { get; private set; }

    public bool IsValid { get; }

    public readonly MatrixDimension Dimension;

    public MatrixValue(MatrixDimension dimension) : this(dimension, false) { }

    public MatrixValue(MatrixDimension dimension, bool triviallyAchieved) : base(triviallyAchieved) {
        this.Dimension = dimension;
        IsValid = Validate();
    }

    private bool Validate() {
        // Check that the matrix is uniform size wise
        List<int> dimensionSize = new();

        MatrixDimension checkDimension = Dimension;

        while (true) {
            if (checkDimension.Length == 0) {
                return false;
            }

            dimensionSize.Add(checkDimension.Length);
            IMatrixElement first = checkDimension.First();
            if (first is not MatrixDimension md) {
                break;
            }

            checkDimension = md;
        }

        bool ValidateRecursive(MatrixDimension dimension, int depth) {
            if (depth >= dimensionSize.Count) {
                return false;
            }
            if (dimension.Length != dimensionSize[depth]) {
                return false;
            }
            foreach (IMatrixElement element in dimension) {
                if (element is MatrixDimension md) {
                    if (ValidateRecursive(md, depth+1) == false) {
                        return false;
                    }
                }
                if (element is MatrixValueElement) {
                    if (depth != dimensionSize.Count -1) {
                        return false;
                    }
                }
            }
            return true;
        }

        DimensionCount = dimensionSize.Count;

        return ValidateRecursive(Dimension, 0);
    }

    public bool IsVector => DimensionCount == 1;

    public Rational Magnitude(ExecutionContext context, Expression node) {
        if (IsVector == false) {
            throw new InvalidOperationException("Matrix is not a vector");
        }

        Rational total = 0;
        foreach (MatrixValueElement element in Dimension) {
            if (element.Value is IConvertibleToReal ctr) {
                Rational val = ctr.ConvertToReal(context, node).Value;
                total += val*val;
            }
        }

        return RationalUtils.Sqrt(total);
    }


    public override Value Clone(bool? triviallyAchieved = null) {
        return new MatrixValue(Dimension);
    }

    public override bool Equals(Value? other) {
        if (other is not MatrixValue mv) {
            return false;
        }

        return mv.Dimension.Equals(Dimension);
    }

    public override int GetHashCode() {
        return Dimension.GetHashCode();
    }

    public override string ToString() {
        string openSymbol, closeSymbol;
        if (DimensionCount == 1) {
            openSymbol = "(";
            closeSymbol = ")";
        } else {
            openSymbol = "[";
            closeSymbol = "]";
        }

        StringBuilder sb = new();

        void RecurseBuild(MatrixDimension dimension) {
            sb.Append(openSymbol);

            bool first = true;
            foreach (IMatrixElement element in dimension) {
                if (first == false) {
                    sb.Append(", ");
                }
                first = false;
                if (element is MatrixDimension md) {
                    RecurseBuild(md);
                } else if (element is MatrixValueElement mve) {
                    sb.Append(mve.Value.ToString());
                }
            }

            sb.Append(closeSymbol);
        }
        RecurseBuild(Dimension);

        return sb.ToString();
    }

    private static bool SameDimensions(MatrixValue matrix0, MatrixValue matrix1) {
        MatrixDimension? dimension0 = matrix0.Dimension;
        MatrixDimension? dimension1 = matrix1.Dimension;
        while (true) {
            if (dimension0.Length != dimension1.Length) {
                return false;
            }

            dimension0 = dimension0[0] as MatrixDimension;
            dimension1 = dimension1[0] as MatrixDimension;

            if (dimension0 != null && dimension1 != null) {
                continue;
            }
            return dimension0 == null && dimension1 == null;
        }
    }
}
