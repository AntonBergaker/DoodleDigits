using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core;
public class Constant {

    public readonly string Name;
    public readonly Value Value;

    public Constant(string name, Value value) {
        Name = name;
        Value = value;
    }
}

public static class ConstantLibrary {

    public static Constant[] Constants = {
        new("true", new BooleanValue(true, triviality: ValueTriviality.Trivial, BooleanValue.PresentationForm.Unset)),
        new("false", new BooleanValue(false, triviality: ValueTriviality.Trivial, BooleanValue.PresentationForm.Unset)),
        new("pi", new RealValue(RationalUtils.Pi, ValueTriviality.NonTrivial, RealValue.PresentedForm.Unset)),
        new("π", new RealValue(RationalUtils.Pi, ValueTriviality.NonTrivial, RealValue.PresentedForm.Unset)),
        new("tau", new RealValue(RationalUtils.Tau, ValueTriviality.NonTrivial, RealValue.PresentedForm.Unset)),
        new("e", new RealValue(RationalUtils.EulersNumber, ValueTriviality.NonTrivial, RealValue.PresentedForm.Unset)),
        new("infinity", new TooBigValue(TooBigValue.Sign.PositiveInfinity, ValueTriviality.Trivial)),
        new("∞", new TooBigValue(TooBigValue.Sign.PositiveInfinity, ValueTriviality.Trivial)),
    };


}
