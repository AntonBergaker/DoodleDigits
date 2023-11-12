using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Named;
public static partial class NamedFunctions {


    #region Hardcoded constants

    private static readonly Rational TauFourth = RationalUtils.Tau / 4;
    private static readonly Rational TauHalf = RationalUtils.Tau / 2;
    private static readonly Rational TauThreeFourths = 3 * RationalUtils.Tau / 4;

    private static readonly Rational DegreeToRadiansMod = RationalUtils.Tau / 360;
    private static readonly Rational RadiansToDegreesMod = 360 / RationalUtils.Tau;


    private static Rational CorrectUnitForInput(Rational value, ExecutorContext context) {
        return CorrectUnitDegreesToRadians(value, context).Modulus(RationalUtils.Tau);
    }

    private static Rational CorrectUnitDegreesToRadians(Rational value, ExecutorContext context) {
        if (context.Settings.AngleUnit == AngleUnits.Degrees) {
            return value * DegreeToRadiansMod;
        }
        return value;
    }
    private static Value CorrectUnitForOutput(Value value, ExecutorContext context) {
        if (value is not RealValue realValue) {
            return value;
        }
        if (context.Settings.AngleUnit == AngleUnits.Degrees) {
            return realValue.Clone(value: realValue.Value * RadiansToDegreesMod);
        }
        return value;
    }


    #endregion

    /// <summary>
    /// Runs the provided function with the result x taken as 1/x
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <param name="trigFunction"></param>
    /// <returns></returns>
    private static Value TrigReciprocal(Value value, ExecutorContext context, Function node, Func<Value, ExecutorContext, Function, Value> trigFunction) {

        Value result = trigFunction(value, context, node);

        if (result is IConvertibleToReal convertibleToReal) {
            RealValue realResult = convertibleToReal.ConvertToReal(context, node);
            if (realResult.Value.IsZero == false) {
                return new RealValue(1 / realResult.Value, false, realResult.Form);
            }
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    /// <summary>
    /// Runs the provided function with the arguments as 1/x
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <param name="trigArcFunction"></param>
    /// <returns></returns>
    private static Value TrigArcReciprocal(Value value, ExecutorContext context, Function node, Func<Value, ExecutorContext, Function, Value> trigArcFunction) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            if (realValue.Value.IsZero == false) {
                RealValue real = new RealValue(1 / realValue.Value, false, realValue.Form);

                return trigArcFunction(real, context, node);
            }
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    #region Non Hyperbolic Functions

    [CalculatorFunction(FunctionExpectedType.Real, "sin")]
    public static Value Sine(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            Rational rational = CorrectUnitForInput(realValue.Value, context);

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            if (rational == TauFourth) {
                return new RealValue(Rational.One, false, realValue.Form);
            }

            if (rational == TauHalf) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            if (rational == TauThreeFourths) {
                return new RealValue(-Rational.One, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Sin((double) rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "cos")]
    public static Value Cosine(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            Rational rational = CorrectUnitForInput(realValue.Value, context);

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.One, false, realValue.Form);
            }
            if (rational == TauFourth) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }
            if (rational == TauHalf) {
                return new RealValue(-Rational.One, false, realValue.Form);
            }
            if (rational == TauThreeFourths) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Cos((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "tan")]
    public static Value Tangent(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

            Rational rational = (CorrectUnitDegreesToRadians(realValue.Value, context) + TauFourth).Modulus(RationalUtils.Pi) - TauFourth;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }
            if (rational == TauFourth) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }
            if (rational == -TauFourth) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return RealValue.FromDouble(Math.Tan((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "sec")]
    public static Value Secant(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, Cosine);

    [CalculatorFunction(FunctionExpectedType.Real, "csc", "cosec")]
    public static Value Cosecant(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, Sine);

    [CalculatorFunction(FunctionExpectedType.Real, "cot", "cotan", "ctg")]
    public static Value Cotangent(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, Tangent);

    [CalculatorFunction(FunctionExpectedType.Real, "arcsin", "asin")]
    public static Value ArcSine(Value value, ExecutorContext context, Function node) {
        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

        return CorrectUnitForOutput(RealValue.FromDouble(Math.Asin((double)realValue.Value), false, realValue.Form), context);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arccos", "acos")]
    public static Value ArcCosine(Value value, ExecutorContext context, Function node) {
        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);

        return CorrectUnitForOutput(RealValue.FromDouble(Math.Acos((double)realValue.Value), false, realValue.Form), context);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arctan", "atan")]
    public static Value ArcTangent(Value value, ExecutorContext context, Function node) {
        if (value is not IConvertibleToReal convertibleToReal) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
        var rational = realValue.Value;

        Value returnValue;
        if (rational == Rational.Zero) {
            returnValue = new RealValue(Rational.Zero, false, realValue.Form);
        }
        else if (rational == Rational.One) {
            returnValue = new RealValue(TauFourth/2, false, realValue.Form);
        } else {
            returnValue = RealValue.FromDouble(Math.Atan((double)rational), false, realValue.Form);
        }

        return CorrectUnitForOutput(returnValue, context);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arcsec", "asec")]
    public static Value ArcSecant(Value value, ExecutorContext context, Function node) =>
        TrigArcReciprocal(value, context, node, ArcCosine);

    [CalculatorFunction(FunctionExpectedType.Real, "arccsc", "arccosec", "acsc", "acosec")]
    public static Value ArcCosecant(Value value, ExecutorContext context, Function node) =>
        TrigArcReciprocal(value, context, node, ArcSine);
    [CalculatorFunction(FunctionExpectedType.Real, "arccot", "arccotan", "arcctg", "acot", "acotan", "actg")]
    public static Value ArcCotangent(Value value, ExecutorContext context, Function node) {
        Value result = ArcTangent(value, context, node);
        if (result is IConvertibleToReal resultReal) {
            RealValue realValue = resultReal.ConvertToReal(context, node);
            Rational offset = context.Settings.AngleUnit == AngleUnits.Radians ? TauFourth : 90;

            return new RealValue(offset - realValue.Value, false, realValue.Form);
        }
    

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    #endregion


    #region Hyperbolic functions
    [CalculatorFunction(FunctionExpectedType.Real, "sinh")]
    public static Value SineHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;
            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero);
            }

            return RealValue.FromDouble(Math.Sinh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "cosh")]
    public static Value CosineHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.One);
            }

            return RealValue.FromDouble(Math.Cosh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "tanh")]
    public static Value TangentHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Tanh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "sech")]
    public static Value SecantHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, CosineHyperbolic);

    [CalculatorFunction(FunctionExpectedType.Real, "csch", "cosech")]
    public static Value CosecantHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, SineHyperbolic);

    [CalculatorFunction(FunctionExpectedType.Real, "coth", "cotanh", "ctgh")]
    public static Value CotangentHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigReciprocal(value, context, node, TangentHyperbolic);

    [CalculatorFunction(FunctionExpectedType.Real, "arcsinh", "asinh")]
    public static Value ArcSineHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Asinh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arccosh", "acosh")]
    public static Value ArcCosineHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.One) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Acosh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arctanh", "atanh")]
    public static Value ArcTangentHyperbolic(Value value, ExecutorContext context, Function node) {
        if (value is IConvertibleToReal convertibleToReal) {
            RealValue realValue = ConvertArgumentToReal(convertibleToReal, context, node, 0);
            Rational rational = realValue.Value;

            // Hardcoded to avoid double-unperfectness
            if (rational == Rational.Zero) {
                return new RealValue(Rational.Zero, false, realValue.Form);
            }

            return RealValue.FromDouble(Math.Atanh((double)rational), false, realValue.Form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    [CalculatorFunction(FunctionExpectedType.Real, "arcsech", "asech")]
    public static Value ArcSecantHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigArcReciprocal(value, context, node, ArcCosineHyperbolic);

    [CalculatorFunction(FunctionExpectedType.Real, "arccsch", "arccosech", "acsch", "acosech")]
    public static Value ArcCosecantHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigArcReciprocal(value, context, node, ArcSineHyperbolic);

    [CalculatorFunction(FunctionExpectedType.Real, "arccoth", "arccotanh", "arcctgh", "acoth", "acotanh", "actgh")]
    public static Value ArcCotangentHyperbolic(Value value, ExecutorContext context, Function node) =>
        TrigArcReciprocal(value, context, node, ArcTangentHyperbolic);

    #endregion
}
