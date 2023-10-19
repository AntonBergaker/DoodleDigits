using System;
using System.Linq;
using System.Text;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public partial class RealValue : Value, IConvertibleToReal, IConvertibleToBool {
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
            return ToString(50, 20, "E");
        }

        public string ToString(int maxNumberOfDigits, int scientificDecimals, string exponentCharacter = "E") {
            int magnitude = Value.Magnitude;
            int numOfDigits = Math.Abs(magnitude);

            int @base = Form switch {
                PresentedForm.Decimal => 10,
                PresentedForm.Binary => 2,
                PresentedForm.Hex => 16,
                _ => 10
            };

            if (numOfDigits > maxNumberOfDigits) {
                return Value.ToScientificString(scientificDecimals, @base, exponentCharacter);
            }

            return Value.ToDecimalString(scientificDecimals, @base);
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

        public static Value FromDouble(double value, bool triviallyAchieved, RealValue.PresentedForm form, bool resultOfInfinity = false) {
            if (double.IsPositiveInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.PositiveInfinity : TooBigValue.Sign.Positive);
            }

            if (double.IsNegativeInfinity(value)) {
                return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.NegativeInfinity : TooBigValue.Sign.Negative);
            }

            if (double.IsNaN(value)) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new RealValue(RationalUtils.FromDouble(value), triviallyAchieved, form);
        }

        public override Value Clone(bool? triviallyAchieved = null) {
            return new RealValue(Value, triviallyAchieved ?? this.TriviallyAchieved, Form);
        }

        public RealValue Clone(Rational? value = null, bool? triviallyAchieved = null, AstNode? sourceAstNode = null, PresentedForm? form = null) {
            value ??= this.Value;
            triviallyAchieved ??= this.TriviallyAchieved;
            form ??= this.Form;
            if (triviallyAchieved == this.TriviallyAchieved && form == this.Form && this.Value == value) {
                return this;
            }

            return new RealValue(value.Value, triviallyAchieved.Value, form.Value);
        }

        public BooleanValue ConvertToBool(ExecutionContext context, Expression node) {
            BooleanValue newValue = new BooleanValue(Value > new Rational(1, 2));
            context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, node.Position));
            return newValue;
        }

        public RealValue Round(ExecutionContext context, Expression node) {
            if (HasDecimal == false) {
                return this;
            }

            RealValue rounded = new RealValue(RationalUtils.Round(Value));
            context.AddResult(new ResultConversion(this, rounded, ResultConversion.ConversionType.Rounding, node.Position));
            return rounded;
        }

        public bool HasDecimal => Value.FractionPart != 0;

        public RealValue ConvertToReal(ExecutionContext context, Expression node) {
            return this;
        }
    }
}
