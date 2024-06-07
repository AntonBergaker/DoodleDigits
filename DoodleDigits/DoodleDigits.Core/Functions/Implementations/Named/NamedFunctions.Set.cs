using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Parsing.Ast;
using System.Diagnostics.CodeAnalysis;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Functions.Implementations.Binary;
using Rationals;
using System;

namespace DoodleDigits.Core.Functions.Implementations.Named;
public static partial class NamedFunctions {

    private static void TurnSetOrMatrixToSet(ref Value[] values, ref Expression[] argumentExpressions) {
        if (values.Length != 1) {
            return;
        }
        if (values[0] is not MatrixValue matrix || matrix.DimensionCount != 1) {
            return;
        }

        // Convert values if matrix
        values = matrix.Dimension.OfType<MatrixValue.MatrixValueElement>().Select(x => x.Value).ToArray();
        // And expressions. Fall back to all point to matrix if it's not big enough
        if (argumentExpressions[0] is VectorDeclaration vectorExpression && vectorExpression.Expressions.Length >= values.Length) {
            argumentExpressions = vectorExpression.Expressions;
        } else {
            var first = argumentExpressions[0];
            argumentExpressions = values.Select(_ => first).ToArray();
        }
        if (values.Length != argumentExpressions.Length) {
            ;
        }
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "max")]
    public static Value Max(Value[] values, ExecutorContext context, Function node) {
        var arguments = node.Arguments;
        TurnSetOrMatrixToSet(ref values, ref arguments);

        Value max = values[0];
        Expression maxNode = arguments[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            var childNode = arguments[index];
            var result = BinaryOperations.LessThan(max, value, context, new BinaryNodes(node, maxNode, childNode));
            if (result is BooleanValue { Value: true }) {
                max = value;
                maxNode = childNode;
            }
        }

        return max;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "min")]
    public static Value Min(Value[] values, ExecutorContext context, Function node) {
        var arguments = node.Arguments;
        TurnSetOrMatrixToSet(ref values, ref arguments);

        Value min = values[0];
        Expression minNode = arguments[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            var childNode = arguments[index];
            var result = BinaryOperations.GreaterThan(min, value, context, new BinaryNodes(node, minNode, childNode));
            if (result is BooleanValue { Value: true }) {
                min = value;
                minNode = childNode;
            }
        }

        return min;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "sum")]
    public static Value Sum(Value[] values, ExecutorContext context, Function node) {
        var arguments = node.Arguments;
        TurnSetOrMatrixToSet(ref values, ref arguments);

        Value sum = values[0];

        for (int index = 1; index < values.Length; index++) {
            Value? value = values[index];

            sum = BinaryOperations.Add(sum, value, context, new BinaryNodes(node, node, arguments[index]));
        }

        return sum;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "average")]
    public static Value Average(Value[] values, ExecutorContext context, Function node) {
        var sum = Sum(values, context, node);

        if (sum is RealValue rv) {
            var divisor = values.Length;
            if (values.Length == 1 && values[0] is MatrixValue matrix) {
                divisor = matrix.Dimension.Length;
            }
            return rv.Clone(rv.Value / divisor);
        }

        return sum;
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "median")]
    public static Value Median(Value[] values, ExecutorContext context, Function node) {
        var arguments = node.Arguments;
        TurnSetOrMatrixToSet(ref values, ref arguments);

        var sortedValues = new List<(Value value, Expression node)>();

        for (int index = 0; index < values.Length; index++) {
            Value value = values[index];

            sortedValues.Add((value, arguments[index]));
        }

        if (sortedValues.Count == 0) {
            context.AddResult(new ResultError("No sortable numbers were passed to the function", node.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        sortedValues.Sort((a, b) => {
            var nodes = new BinaryNodes(node, a.node, b.node);

            if (BinaryOperations.Equals(a.value, b.value, context, nodes) is BooleanValue { Value: true }) {
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
            var a = sortedValues[sortedValues.Count / 2 - 1];
            var b = sortedValues[sortedValues.Count / 2];

            return BinaryOperations.Divide(
                BinaryOperations.Add(a.value, b.value, context, new BinaryNodes(node, a.node, b.node)),
                new RealValue(2), context, new BinaryNodes(node, a.node, node)
            );
        }
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "standard_deviation")]
    public static Value StandardDeviation(Value[] values, ExecutorContext context, Function node) {
        var average = Average(values, context, node);

        var arguments = node.Arguments;
        TurnSetOrMatrixToSet(ref values, ref arguments);

        Value? sumOfDeviations = null;
        for (int i = 0; i < values.Length; i++) {
            var diff = BinaryOperations.Subtract(values[i], average, context, new(node, node, arguments[i]));

            var square = BinaryOperations.Multiply(diff, diff, context, new BinaryNodes(node, arguments[i], arguments[i]));
            if (sumOfDeviations == null) {
                sumOfDeviations = square;
            } else {
                sumOfDeviations = BinaryOperations.Add(sumOfDeviations, square, context, new BinaryNodes(node, node, arguments[i]));
            }
        }
        if (sumOfDeviations == null) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        var deviationsAverage = BinaryOperations.Divide(sumOfDeviations, new RealValue(arguments.Length), context, new(node, node, node));

        return Root(deviationsAverage, new RealValue(2), context, new Function("", node, node));
    }
}
