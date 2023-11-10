using System;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes; 
public partial class TooBigValue : Value, IConvertibleToBool {
    public enum Sign {
        Positive,
        PositiveInfinity,
        Negative,
        NegativeInfinity,
    }

    public readonly Sign ValueSign;

    public TooBigValue(Sign sign, bool triviallyAchieved) : base(triviallyAchieved) {
        ValueSign = sign;
    }

    public TooBigValue(Sign sign) : this(sign, false) { }

    public override string ToString() {
        return "Very big";
    }

    public override bool Equals(Value? other) {
        if (other is TooBigValue tbOther) {
            return tbOther.ValueSign == ValueSign;
        }

        return false;
    }

    public override int GetHashCode() {
        return ValueSign.GetHashCode();
    }

    public override Value Clone(bool? triviallyAchieved = null) {
        return new TooBigValue(this.ValueSign, triviallyAchieved ?? TriviallyAchieved);
    }

    public bool IsPositive => ValueSign is Sign.Positive or Sign.PositiveInfinity;

    public Value Negate() {
        return new TooBigValue(ValueSign switch {
            Sign.Positive => Sign.Negative,
            Sign.PositiveInfinity => Sign.NegativeInfinity,
            Sign.Negative => Sign.Positive,
            Sign.NegativeInfinity => Sign.PositiveInfinity,
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    public BooleanValue ConvertToBool(ExecutionContext context, Expression node) {
        BooleanValue newValue = new BooleanValue(IsPositive);
        context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, node.Position));
        return newValue;
    }

    public int GetSimplifiedSize() {
        return ValueSign switch {
            Sign.PositiveInfinity => 2,
            Sign.Positive => 1,
            Sign.Negative => -1,
            Sign.NegativeInfinity => -2,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
