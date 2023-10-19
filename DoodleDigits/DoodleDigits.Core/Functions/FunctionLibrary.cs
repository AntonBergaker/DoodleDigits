using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Functions.Implementations.Named;

namespace DoodleDigits.Core {
    public class FunctionData {
        public readonly string[] Names;

        public readonly FunctionExpectedType ExpectedType;

        public readonly Func<Value[], ExecutionContext, Function, Value> Function;

        public readonly Range ParameterCount;

        public FunctionData(string[] names, FunctionExpectedType type, Range parameterCount, Func<Value[], ExecutionContext, Function, Value> function) {
            Names = names;
            ExpectedType = type;
            this.ParameterCount = parameterCount;
            Function = function;
        }


        public FunctionData(string[] names, FunctionExpectedType type, Func<Value, ExecutionContext, Function, Value> function) : this(
            names, type, 1..1, (parameters, context, nodes) => function(parameters[0], context, nodes)
        ) { }

        public FunctionData(string[] names, FunctionExpectedType type, Func<Value, Value, ExecutionContext, Function, Value> function) : this(
                names, type, 2..2, (parameters, context, nodes) => function(parameters[0], parameters[1], context, nodes)
            ) { }

        public FunctionData(string[] names, FunctionExpectedType type, Func<Value, Value, Value, ExecutionContext, Function, Value> function) : this(
            names, type, 3..3, (parameters, context, nodes) => function(parameters[0], parameters[1], parameters[2], context, nodes)
        ) { }

    }

    public static partial class FunctionLibrary {
        // This array is populated by the function source generator
        public static FunctionData[] Functions { get; } = null!;
    }
}
