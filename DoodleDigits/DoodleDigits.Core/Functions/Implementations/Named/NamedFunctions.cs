using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Named; 
public static partial class NamedFunctions {

    private static RealValue ConvertArgumentToReal(IConvertibleToReal value, ExecutionContext context, Function node, int index) {
        return value.ConvertToReal(context, node.Arguments[index]);
    }
    
    [CalculatorFunction(FunctionExpectedType.Real, 1, 2, "log")]
    public static Value Log(Value[] values, ExecutionContext context, Function node) {
        if (values[0] is not IConvertibleToReal convertibleToReal0) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        var value = ConvertArgumentToReal(convertibleToReal0, context, node, 0);

        if (value.Value <= Rational.Zero) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
        }

        if (values.Length == 1) {
            return RealValue.FromDouble(Rational.Log10( value.Value ), false, value.Form);
        }

        if (values[1] is TooBigValue {IsPositive: true}) {
            return new RealValue(Rational.Zero, false, value.Form);
        }

        if (values[1] is not IConvertibleToReal convertibleToReal1) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        var @base = ConvertArgumentToReal(convertibleToReal1, context, node, 1);

        return RealValue.FromDouble(Rational.Log(
            value.Value,
            (double)@base.Value
        ), false, value.Form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "root")]
    public static Value Root(Value value, Value root, ExecutionContext context, Function node) {

        if (value is IConvertibleToReal rValue && root is IConvertibleToReal rRoot) {
            var dValue = ConvertArgumentToReal(rValue, context, node, 0);
            var dRoot = ConvertArgumentToReal(rRoot, context, node, 1);

            if (dRoot.Value == Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            if (dValue.Value < Rational.Zero && dRoot.Value.Modulus(2) == 0) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return RealValue.FromDouble(Math.Pow((double) dValue.Value, (double) (1 / dRoot.Value)), false, dValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "ln")]
    public static Value Ln(Value value, ExecutionContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            if (realValue.Value <= Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return RealValue.FromDouble(Rational.Log(realValue.Value), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, 2, int.MaxValue, "gcd", "gcf")]
    public static Value GreatestCommonDivisor(Value[] values, ExecutionContext context, Function node) {
        List<RealValue> realValues = new();
        for (int i = 0; i < values.Length; i++) {
            if (values[i] is not IConvertibleToReal valueCtr) {
                continue;
            }

            realValues.Add(valueCtr.ConvertToReal(context, node).Round(context, node.Arguments[i]));
        }

        if (realValues.Count == 0) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue firstValue = realValues.First();
        BigInteger value = firstValue.Value.Numerator;

        for (int i = 1; i < realValues.Count; i++) {
            RealValue realValue = realValues[i];
            value = BigInteger.GreatestCommonDivisor(value, realValue.Value.Numerator);
        }

        return new RealValue(value, false, firstValue.Form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "sqrt", "square_root")]
    public static Value Sqrt(Value value, ExecutionContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            if (realValue.Value < Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new RealValue(RationalUtils.Sqrt(realValue.Value), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real | FunctionExpectedType.Vector, "abs", "absolute")]
    public static Value Absolute(Value value, ExecutionContext context, Function node) {
        if (value is TooBigValue tbv) {
            return tbv.IsPositive ? tbv : tbv.Negate();
        }

        if (value is MatrixValue) {
            return VectorFunctions.Magnitude(value, context, node);
        }

        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            return new RealValue(Rational.Abs(realValue.Value), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "sign", "sig")]
    public static Value Sign(Value value, ExecutionContext context, Function node) {
        if (value is TooBigValue tbv) {
            return tbv.IsPositive ? new RealValue(Rational.One) : new RealValue(-1);
        }

        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            return new RealValue(realValue.Value.Sign, false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "floor")]
    public static Value Floor(Value value, ExecutionContext context, Function node) {
        if (value is TooBigValue) {
            return value;
        }

        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
        return new RealValue(RationalUtils.Floor(realValue.Value), false, realValue.Form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "round")]
    public static Value Round(Value value, ExecutionContext context, Function node) {
        if (value is TooBigValue) {
            return value;
        }

        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
        return new RealValue(RationalUtils.Round(realValue.Value), false, realValue.Form);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "ceil", "ceiling")]
    public static Value Ceil(Value value, ExecutionContext context, Function node) {
        if (value is TooBigValue) {
            return value;
        }

        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
        return new RealValue(RationalUtils.Ceil(realValue.Value), false, realValue.Form);
    }
}
