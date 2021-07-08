using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.ValueTypes;
using Rationals;

namespace DoodleDigits.Core.Execution {
    public abstract class Value {
        public abstract override string ToString();

        public static Value FromDouble(double value, bool resultOfInfinity = false) {
            if (double.IsPositiveInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.PositiveInfinity : TooBigValue.Sign.Positive);
            }

            if (double.IsNegativeInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.NegativeInfinity : TooBigValue.Sign.Negative);
            }

            if (double.IsNaN(value)) {
                return new UndefinedValue();
            }

            return new RealValue((Rational) value);
        }

        /// <summary>
        /// Determines if the value is of an abstract type, with no real mathematical value. Abstract values include undefined and "too big".
        /// </summary>
        public abstract bool IsAbstract { get; }
    }
}
