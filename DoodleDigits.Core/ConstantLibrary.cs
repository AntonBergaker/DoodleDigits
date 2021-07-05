using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;

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
            new("pi", new RealValue(Math.PI)),
            new("tau", new RealValue(Math.Tau)),
            new("e", new RealValue(Math.E)),
        };


    }
}
