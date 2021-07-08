using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
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
            new("true", new BooleanValue(true)),
            new("false", new BooleanValue(false)),
            new("pi", new RealValue((Rational) Math.PI)),
            new("tau", new RealValue((Rational) Math.Tau)),
            new("e", new RealValue((Rational) Math.E)),
        };


    }
}
