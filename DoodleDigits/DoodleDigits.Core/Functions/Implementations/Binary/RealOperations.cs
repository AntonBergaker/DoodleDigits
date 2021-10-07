using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary {

    public static partial class BinaryOperations {
        private static (RealValue lhs, RealValue rhs) ConvertToReal(IConvertibleToReal lhs, IConvertibleToReal rhs,
            ExecutionContext<BinaryOperation> context) {
            return (
                lhs.ConvertToReal(context.ForNode(context.Node.Lhs)),
                rhs.ConvertToReal(context.ForNode(context.Node.Rhs))
            );
        }

        public static Value Add(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }

            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);
                return new RealValue((result.lhs.Value + result.rhs.Value).CanonicalForm, false, result.lhs.Form);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value Subtract(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue tbvRhs) {
                return tbvRhs.Negate();
            }

            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);
                return new RealValue((result.lhs.Value - result.rhs.Value).CanonicalForm, false, result.lhs.Form);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }


        public static Value Divide(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                int sign = (tbLhs.IsPositive ? 1 : -1) * (tbRhs.IsPositive ? 1 : -1);
                return sign == 1 ? tbLhs : tbLhs.Negate();
            }

            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return new RealValue(0);
            }

            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);

                if (result.rhs.Value == Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return new RealValue((result.lhs.Value / result.rhs.Value).CanonicalForm, false, result.lhs.Form);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value Multiply(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            // Vector calculations
            if (lhs is MatrixValue lhsMatrix && rhs is MatrixValue rhsMatrix) {
                throw new NotImplementedException();
            }
            
            if (lhs is MatrixValue && rhs is IConvertibleToReal ||
                rhs is MatrixValue && lhs is IConvertibleToReal) {
                
                MatrixValue matrix = lhs as MatrixValue ?? (MatrixValue)rhs;
                IConvertibleToReal ctr = lhs as IConvertibleToReal ?? (IConvertibleToReal)rhs;
                RealValue realValue = ctr.ConvertToReal(context);

                return matrix.SelectAll(x => new RealValue( x.Value * realValue.Value) );
            }

            // 0 checks, 0's always result in 0s
            {
                if (lhs is RealValue rLhs && rLhs.Value == 0) {
                    return rLhs;
                }

                if (rhs is RealValue rRhs && rRhs.Value == 0) {
                    return rRhs;
                }
            }

            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                int sign = (tbLhs.IsPositive ? 1 : -1) * (tbRhs.IsPositive ? 1 : -1);
                return sign == 1 ? tbLhs : tbLhs.Negate();
            }

            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }


            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);
                return new RealValue((result.lhs.Value * result.rhs.Value).CanonicalForm, false, result.lhs.Form);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }


        public static Value Modulus(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            if (rhs is TooBigValue) {
                return lhs;
            }


            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);

                if (result.rhs.Value == Rational.Zero) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
                }

                return new RealValue(result.lhs.Value.Modulus(result.rhs.Value).CanonicalForm, false, result.lhs.Form);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);

        }


        public static Value Power(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) {
            if (lhs is TooBigValue) {
                return lhs;
            }

            if (rhs is TooBigValue) {
                return rhs;
            }


            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                var result = ConvertToReal(ctrLhs, ctrRhs, context);
                return Power(result.lhs, result.rhs);
            }

            if (lhs is UndefinedValue || rhs is UndefinedValue) {
                return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value Power(RealValue lhs, RealValue rhs) {
            if (lhs.Value.IsZero && rhs.Value < Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            if (lhs.Value.IsZero) {
                return new RealValue(Rational.Zero);
            }
            if (rhs.Value.IsZero) {
                return new RealValue(Rational.One);
            }

            if (rhs.HasDecimal == false) {
                // Only calculate if the value isn't too complex as the math would take years
                if (Rational.Abs(lhs.Value.GetComplexity()* rhs.Value) < 20000) {
                    return new RealValue(Rational.Pow(lhs.Value, (int)rhs.Value).CanonicalForm);
                }
            }

            return Value.FromDouble(Math.Pow(lhs.Value.ToDouble(), rhs.Value.ToDouble()), false, lhs.Form);
        }


    }
}
