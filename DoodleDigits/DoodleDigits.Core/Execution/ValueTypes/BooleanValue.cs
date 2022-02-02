using System;
using System.Globalization;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Parsing.Ast;
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

        public BooleanValue(bool value) : this(value, false, null, PresentationForm.Unset) { }

        public BooleanValue(bool value, bool triviallyAchieved, AstNode? sourceAstNode, PresentationForm presentationForm) : base(triviallyAchieved, sourceAstNode) {
            Value = value;
            Form = presentationForm;
        }

        public override string ToString() {
            return Value ? "true" : "false";
        }

        public RealValue ConvertToReal(ExecutionContext context) {
            RealValue newValue = new RealValue(Value ? Rational.One : Rational.Zero);
            Range position = this.SourceAstNode?.Position ?? context.Position;
            context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, position));
            return newValue;
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

        public override Value Clone(bool? triviallyAchieved = null) {
            return new BooleanValue(Value, triviallyAchieved ?? this.TriviallyAchieved, this.SourceAstNode, PresentationForm.Unset);
        }

        public Value Clone(bool? triviallyAchieved = null, AstNode? sourceAstNode = null, PresentationForm? form = null) {
            return new BooleanValue(Value, 
                triviallyAchieved ?? this.TriviallyAchieved,
                sourceAstNode ?? this.SourceAstNode, 
                form ?? this.Form
            );
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
