using System;
using System.Globalization;
using DoodleDigits.Core.Execution.Results;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class BooleanValue : Value, IConvertibleToReal, IConvertibleToBool {
        public enum PresentationForm {
            Unset,
            FromBooleanOperation,
            FromComparison,
        }
        
        public readonly bool Value;

        public readonly PresentationForm Form;

        public BooleanValue(bool value) : this(value, false, PresentationForm.Unset) { }

        public BooleanValue(bool value, bool triviallyAchieved, PresentationForm presentationForm) : base(triviallyAchieved) {
            Value = value;
            Form = presentationForm;
        }

        public override string ToString() {
            return Value ? "true" : "false";
        }

        public RealValue ConvertToReal() {
            return new RealValue(Value ? Rational.One : Rational.Zero);
        }

        public RealValue ConvertToReal(ExecutionContext context) {
            RealValue newValue = ConvertToReal();
            context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, context.Position));
            return newValue;
        }

        public BooleanValue ConvertToBool() {
            return this;
        }

        public BooleanValue ConvertToBool(ExecutionContext context) {
            return this;
        }

        public override bool Equals(Value? other) {
            if (other is not BooleanValue bOther) {
                return false;
            }

            return bOther.Value == Value;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public override Value Clone(bool? triviallyAchieved = null) {
            return new BooleanValue(Value, triviallyAchieved ?? this.TriviallyAchieved, PresentationForm.Unset);
        }

        public Value Clone(bool? triviallyAchieved = null, PresentationForm? form = null) {
            return new BooleanValue(Value, triviallyAchieved ?? this.TriviallyAchieved, form ?? this.Form);
        }
    }
}
