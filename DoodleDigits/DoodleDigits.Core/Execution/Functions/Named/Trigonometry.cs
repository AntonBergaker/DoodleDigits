using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution {
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
                    return new RealValue(1 / realResult.Value);
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
                    RealValue real = new RealValue(1 / realValue.Value);

                    return trigArcFunction(real, context);
                }
            }

            return new UndefinedValue();
        }

        #region Non Hyperbolic Functions

        public static Value Sine(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = realValue.Value.Modulus(RationalUtils.Tau);

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                if (rational == TauFourth) {
                    return new RealValue(Rational.One);
                }

                if (rational == TauHalf) {
                    return new RealValue(Rational.Zero);
                }

                if (rational == TauThreeFourths) {
                    return new RealValue(-Rational.One);
                }

                return Value.FromDouble(Math.Sin((double) rational));
            }

            return new UndefinedValue();
        }

        public static Value Cosine(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = realValue.Value.Modulus(RationalUtils.Tau);

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.One);
                }
                if (rational == TauFourth) {
                    return new RealValue(Rational.Zero);
                }
                if (rational == TauHalf) {
                    return new RealValue(-Rational.One);
                }
                if (rational == TauThreeFourths) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Cos((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value Tangent(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {
                RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

                Rational rational = (realValue.Value + TauFourth).Modulus(RationalUtils.Pi) - TauFourth;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }
                if (rational == TauFourth) {
                    return new UndefinedValue();
                }
                if (rational == -TauFourth) {
                    return new UndefinedValue();
                }

                return Value.FromDouble(Math.Tan((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value Secant(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Cosine);

        public static Value Cosecant(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Sine);

        public static Value Cotangent(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, Tangent);



        public static Value ArcSine(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Asin((double)realValue.Value));
        }

        public static Value ArcCosine(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Acos((double)realValue.Value));
        }

        public static Value ArcTangent(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Atan((double)realValue.Value));
        }

        public static Value ArcSecant(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcCosine);

        public static Value ArcCosecant(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcSine);

        public static Value ArcCotangent(Value value, ExecutionContext<Function> context) {
            Value result = ArcTangent(value, context);
            if (result is IConvertibleToReal resultReal) {
                RealValue realValue = resultReal.ConvertToReal(context);
                return new RealValue(TauFourth - realValue.Value);
            }
        

            return new UndefinedValue();
        }

        #endregion


        #region Hyperbolic functions
        public static Value SineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Sinh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value CosineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.One);
                }

                return Value.FromDouble(Math.Cosh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value TangentHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Tanh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value SecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, CosineHyperbolic);

        public static Value CosecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, SineHyperbolic);

        public static Value CotangentHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigReciprocal(value, context, TangentHyperbolic);


        public static Value ArcSineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Asinh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value ArcCosineHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.One) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Acosh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value ArcTangentHyperbolic(Value value, ExecutionContext<Function> context) {
            if (value is IConvertibleToReal convertibleToReal) {

                Rational rational = ConvertArgumentToReal(convertibleToReal, 0, context).Value;

                // Hardcoded to avoid double-unperfectness
                if (rational == Rational.Zero) {
                    return new RealValue(Rational.Zero);
                }

                return Value.FromDouble(Math.Atanh((double)rational));
            }

            return new UndefinedValue();
        }

        public static Value ArcSecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcCosineHyperbolic);

        public static Value ArcCosecantHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcSineHyperbolic);

        public static Value ArcCotangentHyperbolic(Value value, ExecutionContext<Function> context) =>
            TrigArcReciprocal(value, context, ArcTangentHyperbolic);

        #endregion
    }
}
