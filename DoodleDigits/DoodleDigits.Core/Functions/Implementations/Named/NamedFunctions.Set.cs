using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Parsing.Ast;
using System.Diagnostics.CodeAnalysis;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions.Implementations.Binary;

namespace DoodleDigits.Core.Functions.Implementations.Named;
public static partial class NamedFunctions {

    private static bool TryMatrixToSet(Value[] values, [MaybeNullWhen(false)] out Value[] resultValues) {
        resultValues = null;
        if (values.Length != 1) {
            return false;
        }
        if (values[0] is not MatrixValue matrix || matrix.DimensionCount != 1) {
            return false;
        }

        resultValues = matrix.Dimension.OfType<MatrixValue.MatrixValueElement>().Select(x => x.Value).ToArray();
        return true;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "max")]
    public static Value Max(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        Value max = values[0];
        Expression maxNode = node.Arguments[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            var childNode = node.Arguments[forceIndex ?? index];
            var result = BinaryOperations.LessThan(max, value, context, new BinaryNodes(node, maxNode, childNode));
            if (result is BooleanValue { Value: true }) {
                max = value;
                maxNode = childNode;
            }
        }

        return max;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "min")]
    public static Value Min(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        Value min = values[0];
        Expression minNode = node.Arguments[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            var childNode = node.Arguments[forceIndex ?? index];
            var result = BinaryOperations.GreaterThan(min, value, context, new BinaryNodes(node, minNode, childNode));
            if (result is BooleanValue { Value: true}) {
                min = value;
                minNode = childNode;
            }
        }

        return min;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "sum")]
    public static Value Sum(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        Value sum = values[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            sum = BinaryOperations.Add(sum, value, context, new BinaryNodes(node, node, node.Arguments[forceIndex ?? index]));
        }

        return sum;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "average")]
    public static Value Average(Value[] values, ExecutionContext context, Function node) {
        var sum = Sum(values, context, node);

        if (sum is RealValue rv) {
            return rv.Clone(rv.Value / values.Length);
        }

        return sum;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "median")]
    public static Value Median(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        var sortedValues = new List<(Value value, Expression node)>();

        for (int index = 0; index < values.Length; index++) {
            Value value = values[index];

            sortedValues.Add((value, node.Arguments[forceIndex ?? index]));
        }

        if (sortedValues.Count == 0) {
            context.AddResult(new ResultError("No sortable numbers were passed to the function", node.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        sortedValues.Sort((a, b) => {
            var nodes = new BinaryNodes(node, a.node, b.node);

            if (BinaryOperations.Equals(a.value, b.value, context, nodes) is BooleanValue { Value: true}) {
                return 0;
            }

            var greaterResult = BinaryOperations.GreaterThan(a.value, b.value, context, new BinaryNodes(node, a.node, b.node));
            if (greaterResult is not BooleanValue @bool) {
                return 0;
            }

            return @bool.Value ? 1 : -1;
        });

        // Uneven, nice!
        if (sortedValues.Count % 2 == 1) {
            return sortedValues[sortedValues.Count / 2].value.Clone(triviallyAchieved: false);
        } else {
            var a = sortedValues[sortedValues.Count/2 - 1];
            var b = sortedValues[sortedValues.Count/2];

            return BinaryOperations.Divide( 
                BinaryOperations.Add(a.value, b.value, context, new BinaryNodes(node, a.node, b.node)),
                new RealValue(2), context, new BinaryNodes(node, a.node, node)    
            );
        }

    }
}
