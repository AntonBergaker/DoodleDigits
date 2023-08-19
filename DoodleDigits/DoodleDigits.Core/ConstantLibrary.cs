using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core {
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
            new("true", new BooleanValue(true, triviallyAchieved: true, null, BooleanValue.PresentationForm.Unset)),
            new("false", new BooleanValue(false, triviallyAchieved: true, null, BooleanValue.PresentationForm.Unset)),
            new("pi", new RealValue(RationalUtils.Pi)),
            new("π", new RealValue(RationalUtils.Pi)),
            new("tau", new RealValue(RationalUtils.Tau)),
            new("e", new RealValue(RationalUtils.EulersNumber)),
            new("infinity", new TooBigValue(TooBigValue.Sign.PositiveInfinity)),
            new("∞", new TooBigValue(TooBigValue.Sign.PositiveInfinity, triviallyAchieved: true, null)),
        };


    }
}
