using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Functions.Implementations.Named;

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


        public FunctionData(string[] names, Func<Value, ExecutionContext<Function>, Value> function) : this(
            names, 1..1, (parameters, context) => function(parameters[0], context)
        ) { }

        public FunctionData(string[] names, Func<Value, Value, ExecutionContext<Function>, Value> function) : this(
                names, 2..2, (parameters, context) => function(parameters[0], parameters[1], context)
            ) { }

    }

    public static partial class FunctionLibrary {
        // This array is populated by the function source generator
        public static FunctionData[] Functions { get; } = null!;

    }
}
