using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Functions;
public class FunctionData {
    public readonly string[] Names;

    public readonly FunctionExpectedType ExpectedType;

    public readonly Func<Value[], ExecutorContext, Function, Value> Function;

    public readonly Range ParameterCount;

    public FunctionData(string[] names, FunctionExpectedType type, Range parameterCount, Func<Value[], ExecutorContext, Function, Value> function) {
        Names = names;
        ExpectedType = type;
        this.ParameterCount = parameterCount;
        Function = function;
    }


    public FunctionData(string[] names, FunctionExpectedType type, Func<Value, ExecutorContext, Function, Value> function) : this(
        names, type, 1..1, (parameters, context, nodes) => function(parameters[0], context, nodes)
    ) { }

    public FunctionData(string[] names, FunctionExpectedType type, Func<Value, Value, ExecutorContext, Function, Value> function) : this(
            names, type, 2..2, (parameters, context, nodes) => function(parameters[0], parameters[1], context, nodes)
        ) { }

    public FunctionData(string[] names, FunctionExpectedType type, Func<Value, Value, Value, ExecutorContext, Function, Value> function) : this(
        names, type, 3..3, (parameters, context, nodes) => function(parameters[0], parameters[1], parameters[2], context, nodes)
    ) { }

}

public static partial class FunctionLibrary {
    // This array is populated by the function source generator
    public static FunctionData[] Functions { get; } = null!;
}
