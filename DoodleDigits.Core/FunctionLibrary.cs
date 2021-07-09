using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core {
    public class FunctionData {
        public readonly string[] Names;

        public readonly Func<Value[], ExecutionContext<Function>, Value> Function;

        public readonly Range ParameterCount;

        public FunctionData(string[] names, Range parameterCount, Func<Value[], ExecutionContext<Function>, Value> function) {
            Names = names;
            this.ParameterCount = parameterCount;
            Function = function;
        }

        public FunctionData(string name, Range parameterCount, Func<Value[], ExecutionContext<Function>, Value> function) : 
            this(new[] {name}, parameterCount, function) { }

        public FunctionData(string[] names, Func<Value, ExecutionContext<Function>, Value> function) : this(
            names, 1..1, (parameters, context) => function(parameters[0], context)
        ) { }

        public FunctionData(string name, Func<Value, ExecutionContext<Function>, Value> function) : this(
                new[] {name}, function
            ) { }

        public FunctionData(string[] names, Func<Value, Value, ExecutionContext<Function>, Value> function) : this(
                names, 2..2, (parameters, context) => function(parameters[0], parameters[1], context)
            ) { }

        public FunctionData(string name, Func<Value, Value, ExecutionContext<Function>, Value> function) : this(
            new[] { name }, function
        ) { }


    }

    public static class FunctionLibrary {

        public static FunctionData[] Functions = new FunctionData[] {
            new("log", 1..2, NamedFunctions.Log),
            new("root", NamedFunctions.Root),
            new("ln", NamedFunctions.Ln),
            new("sin", NamedFunctions.Sin),
            new("cos", NamedFunctions.Cos),
            new("tan", NamedFunctions.Tan),
            new(new [] {"sqrt", "square_root"}, NamedFunctions.Sqrt),
            new("max", 1..^1, NamedFunctions.Max),
            new("min", 1..^1, NamedFunctions.Min),
            new("floor", NamedFunctions.Floor),
            new(new [] {"ceil", "ceiling"}, NamedFunctions.Ceil),
            new("round", NamedFunctions.Round),
        };

    }
}
