using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Named {
    public static partial class NamedFunctions {


        #region Hardcoded constants

        private static readonly Rational TauFourth = RationalUtils.Tau / 4;
        private static readonly Rational TauHalf = RationalUtils.Tau / 2;
        private static readonly Rational TauThreeFourths = 3 * RationalUtils.Tau / 4;

        #endregion

        /// <summary>
        /// Runs the provided function with the result x taken as 1/x
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <param name="trigFunction"></param>
        /// <returns></returns>
        private static Value TrigReciprocal(Value value, ExecutionContext<Function> context, Func<Value, ExecutionContext<Function>, Value> trigFunction) {

            Value result = trigFunction(value, context);

            if (result is IConvertibleToReal convertibleToReal) {
                RealValue realResult = convertibleToReal.ConvertToReal(context);
                if (realResult.Value.IsZero == false) {
                    return new RealValue(1 / realResult.Value, false, realResult.Form);
                }
            }

            return new UndefinedValue();
        }

        /// <summary>
        /// Runs the provided function with the arguments as 1/x
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <param name="trigArcFunction"></param>
        /// <returns></returns>
        private static Value TrigArcReciprocal(Value value, ExecutionContext<Function> context, Func<Value, ExecutionContext<Function>, Value> trigArcFunction) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                if (realValue.Value.IsZero == false) {
                    RealValue real = new RealValue(1 / realValue.Value, false, realValue.Form);

                    return trigArcFunction(real, context);
                }
            }

            return new UndefinedValue();
        }

        #region Non Hyperbolic Functions

        [CalculatorFunction("sin")]
        public static Value Sine(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = realValue.Value.Modulus(RationalUtils.Tau);

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

                return Value.FromDouble(Math.Sin((double) rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("cos")]
        public static Value Cosine(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = realValue.Value.Modulus(RationalUtils.Tau);

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

                return Value.FromDouble(Math.Cos((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("tan")]
        public static Value Tangent(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = (realValue.Value + TauFourth).Modulus(RationalUtils.Pi) - TauFourth;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero, false, realValue.Form);
                }
                if (rational == TauFourth) {
                    return new UndefinedValue();
                }
                if (rational == -TauFourth) {
                    return new UndefinedValue();
                }

                return Value.FromDouble(Math.Tan((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("sec")]
        public static Value Secant(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Cosine);

        [CalculatorFunction("csc", "cosec")]
        public static Value Cosecant(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Sine);

        [CalculatorFunction("cot", "cotan", "ctg")]
        public static Value Cotangent(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Tangent);

        [CalculatorFunction("arcsin", "asin")]
        public static Value ArcSine(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Asin((double)realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("arccos", "acos")]
        public static Value ArcCosine(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Acos((double)realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("arctan", "atan")]
        public static Value ArcTangent(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Atan((double)realValue.Value), false, realValue.Form);
        }

        [CalculatorFunction("arcsec", "asec")]
        public static Value ArcSecant(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcCosine);

        [CalculatorFunction("arccsc", "arccosec", "acsc", "acosec")]
        public static Value ArcCosecant(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcSine);
        [CalculatorFunction("arccot", "arccotan", "arcctg", "acot", "acotan", "actg")]
        public static Value ArcCotangent(Value value, ExecutionContext<Function> context) {
            Value result = ArcTangent(value, context);
            if (result is IConvertibleToReal resultReal) {
                RealValue realValue = resultReal.ConvertToReal(context);
                return new RealValue(TauFourth - realValue.Value, false, realValue.Form);
            }
        

            return new UndefinedValue();
        }

        #endregion


        #region Hyperbolic functions
        [CalculatorFunction("sinh")]
        public static Value SineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;
                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Sinh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("cosh")]
        public static Value CosineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.One);
                }

                return Value.FromDouble(Math.Cosh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("tanh")]
        public static Value TangentHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Tanh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("sech")]
        public static Value SecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, CosineHyperbolic);

        [CalculatorFunction("csch", "cosech")]
        public static Value CosecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, SineHyperbolic);

        [CalculatorFunction("coth", "cotanh", "ctgh")]
        public static Value CotangentHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, TangentHyperbolic);

        [CalculatorFunction("arcsinh", "asinh")]
        public static Value ArcSineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero, false, realValue.Form);
                }

                return Value.FromDouble(Math.Asinh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("arccosh", "acosh")]
        public static Value ArcCosineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.One) {
                    return new RealValue(Rational.Zero, false, realValue.Form);
                }

                return Value.FromDouble(Math.Acosh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("arctanh", "atanh")]
        public static Value ArcTangentHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);
                Rational rational = realValue.Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero, false, realValue.Form);
                }

                return Value.FromDouble(Math.Atanh((double)rational), false, realValue.Form);
            }

            return new UndefinedValue();
        }

        [CalculatorFunction("arcsech", "asech")]
        public static Value ArcSecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcCosineHyperbolic);

        [CalculatorFunction("arccsch", "arccosech", "acsch", "acosech")]
        public static Value ArcCosecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcSineHyperbolic);

        [CalculatorFunction("arccoth", "arccotanh", "arcctgh", "acoth", "acotanh", "actgh")]
        public static Value ArcCotangentHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcTangentHyperbolic);

        #endregion
    }
}
