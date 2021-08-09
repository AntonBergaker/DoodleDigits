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

        public static Value Sin(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

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

            return Value.FromDouble(Math.Sin((double)rational));
        }

        public static Value Cos(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

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

        public static Value Tan(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

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

        public static Value ArcSin(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Asin((double)realValue.Value));
        }

        public static Value ArcCos(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Acos((double)realValue.Value));
        }

        public static Value ArcTan(Value value, ExecutionContext<Function> context) {
            if (value is not IConvertibleToReal convertibleToReal) {
                return new UndefinedValue();
            }

            RealValue realValue = ConvertArgumentToReal(convertibleToReal, 0, context);

            return Value.FromDouble(Math.Atan((double)realValue.Value));
        }

    }
}
