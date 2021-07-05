using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions;

namespace DoodleDigits.Core {
    public class FunctionData {
        public readonly string Name;

        public readonly Func<Value[], ExecutionContext<Function>, Value> Function;

        public readonly Range ParameterCount;

        public FunctionData(string name, Func<Value, ExecutionContext<Function>, Value> function) {
            Name = name;
            ParameterCount = 1..1;
            Function = (parameters, context) => function(parameters[0], context);
        }

        public FunctionData(string name, Func<Value, Value, ExecutionContext<Function>, Value> function) {
            Name = name;
            ParameterCount = 2..2;
            Function = (parameters, context) => function(parameters[0], parameters[1], context);
        }

        public FunctionData(string name, Range parameterCount, Func<Value[], ExecutionContext<Function>, Value> function) {
            Name = name;
            this.ParameterCount = parameterCount;
            Function = function;
        }
    }

    public static class FunctionLibrary {

        public static FunctionData[] Functions = new FunctionData[] {
            new("log", 1..2, NamedFunctions.Log),
            new("root", NamedFunctions.Root),
            new("ln", NamedFunctions.Ln),
            new("sin", NamedFunctions.Sin),
            new("cos", NamedFunctions.Cos),
            new("tan", NamedFunctions.Tan),
            new("sqrt", NamedFunctions.Sqrt),
            new("square_root", NamedFunctions.Sqrt),
            new("max", 1..^1, NamedFunctions.Max),
            new("min", 1..^1, NamedFunctions.Min),
        };

    }
}
