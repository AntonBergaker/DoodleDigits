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

        delegate Value? ImplementationFunction(Value other, BinaryOperation.OperationSide side,
            bool castAttempt, ExecutionContext<BinaryOperation> context);


        private static Value ExecuteBinaryImplementaion(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context, ImplementationFunction lhsMethod, ImplementationFunction rhsMethod) {
            Value? result;
            
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, false, context);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, false, context);
            if (result != null) {
                return result;
            }

            // Try both sides again, but now allow casting
            result = lhsMethod(rhs, BinaryOperation.OperationSide.Left, true, context);
            if (result != null) {
                return result;
            }
            result = rhsMethod(lhs, BinaryOperation.OperationSide.Right, true, context);
            if (result != null) {
                return result;
            }


            if (lhs is UndefinedValue) {
                return lhs;
            }

            if (rhs is UndefinedValue) {
                return rhs;
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value Add(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) => 
            ExecuteBinaryImplementaion(lhs, rhs, context, lhs.TryAdd, rhs.TryAdd);

        public static Value Subtract(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context) =>
            ExecuteBinaryImplementaion(lhs, rhs, context, lhs.TrySubtract, rhs.TrySubtract);



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
                return MatrixValue.Multiply(lhsMatrix, rhsMatrix, context);
            }
            
            // Scalar matrix multiplication
            if (lhs is MatrixValue && rhs is IConvertibleToReal ||
                rhs is MatrixValue && lhs is IConvertibleToReal) {
                
                MatrixValue matrix = lhs as MatrixValue ?? (MatrixValue)rhs;
                RealValue realValue;
                if (lhs is IConvertibleToReal ctr) {
                    realValue = ctr.ConvertToReal(context.ForNode(context.Node.Lhs));
                }
                else {
                    realValue = ((IConvertibleToReal) rhs).ConvertToReal(context.ForNode(context.Node.Rhs));
                }

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
