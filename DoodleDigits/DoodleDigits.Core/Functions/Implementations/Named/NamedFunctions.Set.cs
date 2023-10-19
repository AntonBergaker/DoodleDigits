using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Execution;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Parsing.Ast;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

        Rational? max = null;
        RealValue.PresentedForm form = RealValue.PresentedForm.Unset;

        for (var index = 0; index < values.Length; index++) {
            Value value = values[index];
            if (value is TooBigValue { IsPositive: true }) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                continue;
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, forceIndex ?? index);
            if (max == null) {
                form = realValue.Form;
                max = realValue.Value;
                continue;
            }
            if (realValue.Value > max) {
                max = realValue.Value;
            }
        }

        if (max == null) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }
        return new RealValue(max.Value, false, form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "min")]
    public static Value Min(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        Rational? min = null;
        RealValue.PresentedForm form = RealValue.PresentedForm.Unset;

        for (var index = 0; index < values.Length; index++) {
            Value value = values[index];
            if (value is TooBigValue { IsPositive: false }) {
                return value;
            }

            if (value is not IConvertibleToReal convertibleToReal) {
                continue;
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, forceIndex ?? index);
            if (min == null) {
                min = realValue.Value;
                form = realValue.Form;
                continue;
            }

            if (realValue.Value < min) {
                min = realValue.Value;
            }

        }

        if (min == null) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }
        return new RealValue(min.Value, false, form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, 1, int.MaxValue, "sum")]
    public static Value Sum(Value[] values, ExecutionContext context, Function node) {
        int? forceIndex = null;
        if (TryMatrixToSet(values, out var resultValues)) {
            forceIndex = 0;
            values = resultValues;
        }

        Value sum = new RealValue(0, false, RealValue.PresentedForm.Unset);

        for (int index = 0; index < values.Length; index++) {
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
