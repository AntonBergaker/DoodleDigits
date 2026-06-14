using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes;
public partial class RealValue : Value, IConvertibleToReal, IConvertibleToBool {
    public readonly Rational Value;

    public enum PresentedForm {
        Unset,
        Decimal,
        Binary,
        Hex
    }

    public PresentedForm Form { get; }

    public RealValue(Rational value) : this(value, ValueTriviality.Unknown, PresentedForm.Unset) {
    }

    public RealValue(Rational value, ValueTriviality triviality, PresentedForm form) : base(triviality) {
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

    public static Value FromDouble(double value, ValueTriviality triviality, RealValue.PresentedForm form, bool resultOfInfinity = false) {
        if (double.IsPositiveInfinity(value)) {
            return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.PositiveInfinity : TooBigValue.Sign.Positive);
        }

        if (double.IsNegativeInfinity(value)) {
            return new TooBigValue(resultOfInfinity ? TooBigValue.Sign.NegativeInfinity : TooBigValue.Sign.Negative);
        }

        if (double.IsNaN(value)) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
        }

        return new RealValue(RationalUtils.FromDouble(value), triviality, form);
    }

    public override Value Clone(ValueTriviality? triviality = null) {
        return new RealValue(Value, triviality ?? this.Triviality, Form);
    }

    public RealValue Clone(Rational? value = null, ValueTriviality? triviality = null, AstNode? sourceAstNode = null, PresentedForm? form = null) {
        value ??= this.Value;
        triviality ??= this.Triviality;
        form ??= this.Form;
        if (triviality == this.Triviality && form == this.Form && this.Value == value) {
            return this;
        }

        return new RealValue(value.Value, triviality.Value, form.Value);
    }

    public BooleanValue ConvertToBool(ExecutorContext context, Expression node) {
        BooleanValue newValue = new BooleanValue(Value > new Rational(1, 2));
        context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, node.Position));
        return newValue;
    }

    public RealValue Round(ExecutorContext context, Expression node) {
        if (HasDecimal == false) {
            return this;
        }

        RealValue rounded = new RealValue(RationalUtils.Round(Value));
        context.AddResult(new ResultConversion(this, rounded, ResultConversion.ConversionType.Rounding, node.Position));
        return rounded;
    }

    public bool HasDecimal => Value.FractionPart != 0;

    public RealValue ConvertToReal(ExecutorContext context, Expression node) {
        return this;
    }
}
