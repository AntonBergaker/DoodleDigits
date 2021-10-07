using DoodleDigits.Core.Utilities;
using Rationals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class MatrixValue : Value {
        public interface IMatrixElement { }

        public class MatrixDimension : IMatrixElement, IEnumerable<IMatrixElement> {
            private readonly IMatrixElement[] elements;
            public readonly bool HoldsValues;
            public readonly int Length;

            public IMatrixElement this[int index] => elements[index];

            public MatrixDimension(IEnumerable<IMatrixElement> elements) {
                this.elements = elements.ToArray();
                Length = this.elements.Length;
            }

            public MatrixDimension(params IMatrixElement[] elements) : this((IEnumerable<IMatrixElement>)elements) { }

            public IEnumerator<IMatrixElement> GetEnumerator() {
                return ((IEnumerable<IMatrixElement>)elements).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }

            public override bool Equals(object? obj) {
                if (obj is not MatrixDimension otherDimension) {
                    return false;
                }

                return Enumerable.SequenceEqual(elements, otherDimension.elements);
            }

            public override int GetHashCode() {
                return HashCode.Combine(elements);
            }
        }

        public class MatrixValueElement : IMatrixElement {
            public readonly RealValue Value;

            public MatrixValueElement(RealValue value) {
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
            private IMatrixElement element;

            public MatrixElement this[int index] {
                get {
                    if (element is MatrixDimension md) {
                        return new MatrixElement(md[index]);
                    }

                    throw new InvalidOperationException("Invalid attempt to reach depth of matrix. Check matrix dimensions.");
                }
            }

            public static implicit operator Value(MatrixElement matrixElement) => matrixElement.Value;

            public RealValue Value {
                get {
                    if (element is MatrixValueElement mve) {
                        return mve.Value;
                    }

                    throw new InvalidOperationException("Attempted to access value before reaching bottom of matrix. Check matrix dimensions.");
                }
            }

            public MatrixElement(IMatrixElement element) {
                this.element = element;
            }
        }

        public MatrixElement this[int index] => new MatrixElement(Dimension[index]);

        public int Dimensions { get; private set; }

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

            Dimensions = dimensionSize.Count;

            return ValidateRecursive(Dimension, 0);
        }

        public MatrixValue SelectAll(Func<RealValue, RealValue> func) {
            
            MatrixDimension SelectDimension(MatrixDimension dimension) {
                List<IMatrixElement> elements = new();
                foreach (IMatrixElement element in dimension) {
                    if (element is MatrixDimension dim) {
                        elements.Add(SelectDimension(dim));
                    } else if (element is MatrixValueElement elem) {
                        elements.Add(new MatrixValueElement(func(elem.Value)));
                    }
                }
                return new MatrixDimension(elements);
            }

            return new MatrixValue(SelectDimension(Dimension));
        }

        public bool IsVector => Dimensions == 1;

        public Rational Magnitude() {
            if (IsVector == false) {
                throw new InvalidOperationException("Matrix is not a vector");
            }

            Rational total = 0;
            foreach (MatrixValueElement element in Dimension) {
                if (element.Value is IConvertibleToReal ctr) {
                    Rational val = ctr.ConvertToReal().Value;
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
            throw new NotImplementedException();
        }

        public override string ToString() {
            string openSymbol, closeSymbol;
            if (Dimensions == 1) {
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
    }
}
