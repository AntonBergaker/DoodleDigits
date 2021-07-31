using System;
using System.Linq;
using System.Text;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class RealValue : Value, IConvertibleToReal, IConvertibleToBool {
        public readonly Rational Value;

        public enum PresentedForm {
            Unset,
            Decimal,
            Binary,
            Hex
        }

        public PresentedForm Form { get; }

        public RealValue(Rational value) : this(value, false, PresentedForm.Unset) {
        }

        public RealValue(Rational value, bool triviallyAchieved, PresentedForm form) : base(triviallyAchieved) {
            Value = value;
            Form = form;
        }

        public override string ToString() {
            return ToString(100, 30);
        }

        public string ToString(int maxNumberOfDigits, int scientificDecimals) {
            int magnitude = Value.Magnitude;
            int numOfDigits = Math.Abs(magnitude);

            if (numOfDigits > maxNumberOfDigits) {
                return ToScientificString(scientificDecimals);
            }

            if (HasDecimal == false) {
                return Value.ToString();
            }

            StringBuilder sb = new StringBuilder(numOfDigits);
            var enumerator = Value.Digits;
            int index = 0;
            foreach (char c in enumerator) {
                sb.Append(c);
                if (index == magnitude) {
                    sb.Append(".");
                }

                index++;
            }


            return sb.ToString();
        }

        public string ToScientificString(int decimals = 10) {
            if (Value.IsZero) {
                return "0";
            }
            
            int magnitude = Value.Magnitude;

            StringBuilder sb = new StringBuilder(decimals);
            var enumerator = Value.Digits;
            int index = 0;
            foreach (char c in enumerator) {
                sb.Append(c);
                if (index == 0) {
                    sb.Append(".");
                }

                index++;
                if (index > decimals) {
                    break;
                }
            }

            return $"{sb}E{magnitude}";
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

        public override Value Clone(bool? triviallyAchieved = null) {
            return new RealValue(Value, triviallyAchieved ?? this.TriviallyAchieved, Form);
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
