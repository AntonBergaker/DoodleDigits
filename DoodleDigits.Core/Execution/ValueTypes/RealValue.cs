using System;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class RealValue : Value, IConvertibleToReal, IConvertibleToBool {
        public readonly Rational Value;

        public RealValue(Rational value) {
            Value = value;
        }

        public override string ToString() {
            if (HasDecimal) {
                return ((double)Value).ToString();
            }
            return Value.ToString();
        }
        
        public override bool Equals(Value? other) {
            if (other is not RealValue rOther) {
                return false;
            }

            return rOther.Value == Value;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public BooleanValue ConvertToBool() {
            return new BooleanValue(Value > new Rational(1, 2));
        }

        public BooleanValue ConvertToBool(ExecutionContext context, Range position) {
            BooleanValue newValue = ConvertToBool();
            context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, position));
            return newValue;
        }


        public RealValue Round(ExecutionContext context, Range position) {
            if (HasDecimal == false) {
                return this;
            }

            RealValue rounded = new RealValue(RationalUtils.Round(Value));
            context.AddResult(new ResultConversion(this, rounded,
                ResultConversion.ConversionType.Rounding, position));
            return rounded;
        }


        public bool HasDecimal => Value.FractionPart != 0;
        public RealValue ConvertToReal() {
            return this;
        }

        public RealValue ConvertToReal(ExecutionContext context, Range position) {
            return this;
        }
    }
}
