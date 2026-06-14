using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Parsing.Ast;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes;
public class BooleanValue : Value, IConvertibleToReal, IConvertibleToBool {
    public enum PresentationForm {
        Unset,
        FromBooleanOperation,
        FromComparison,
    }
    
    public readonly bool Value;

    public readonly PresentationForm Form;

    public BooleanValue(bool value) : this(value, ValueTriviality.Unknown, PresentationForm.Unset) { }

    public BooleanValue(bool value, ValueTriviality triviality, PresentationForm presentationForm) : base(triviality) {
        Value = value;
        Form = presentationForm;
    }

    public override string ToString() {
        return Value ? "true" : "false";
    }

    public RealValue ConvertToReal(ExecutorContext context, Expression node) {
        RealValue newValue = new RealValue(Value ? Rational.One : Rational.Zero);
        context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, node.Position));
        return newValue;
    }

    public BooleanValue ConvertToBool(ExecutorContext context, Expression node) {
        return this;
    }

    public override bool Equals(Value? other) {
        if (other is not BooleanValue bOther) {
            return false;
        }

        return bOther.Value == Value;
    }

    public override Value Clone(ValueTriviality? triviality = null) {
        return new BooleanValue(Value, triviality ?? this.Triviality, PresentationForm.Unset);
    }

    public Value Clone(ValueTriviality? triviality = null, PresentationForm? form = null) {
        return new BooleanValue(Value, 
            triviality ?? this.Triviality,
            form ?? this.Form
        );
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }
}
