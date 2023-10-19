using System;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class RealValue {
    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            return new RealValue((this.Value + otherRealValue.Value).CanonicalForm, false, Form);
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            Rational value = side == BinaryOperation.OperationSide.Left ?
                this.Value - otherRealValue.Value : 
                otherRealValue.Value - this.Value;

            return new RealValue(value.CanonicalForm, false, Form);
        }

        return null;
    }

    public override Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            return new RealValue((this.Value * otherRealValue.Value).CanonicalForm, false, Form);
        }

        return null;
    }

    public override Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherRealValue, side);

            if (rhs.Value == Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new RealValue((lhs.Value / rhs.Value).CanonicalForm, false, Form);
        }

        return null;
    }

    public override Value? TryModulus(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherRealValue, side);

            if (rhs.Value == Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            return new RealValue(lhs.Value.Modulus(rhs.Value), false, Form);
        }

        return null;
    }

    public override Value? TryPower(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext context, BinaryNodes nodes) {
        if (BinaryOperationHelpers.TryConvertToReal(other, shouldConvert, side.Flip(), context, nodes, out var otherRealValue)) {
            var (lhs, rhs) = BinaryOperationHelpers.GetLhsRhs(this, otherRealValue, side);

            if (lhs.Value.IsZero && rhs.Value < Rational.Zero) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Undefined);
            }

            if (rhs.Value.IsZero) {
                return new RealValue(Rational.One);
            }

            if (lhs.Value.IsZero) {
                return new RealValue(Rational.Zero);
            }

            if (rhs.HasDecimal == false) {
                // Only calculate if the value isn't too complex as the math would take years
                if (Rational.Abs(lhs.Value.GetComplexity() * rhs.Value) < 20000) {
                    return new RealValue(Rational.Pow(lhs.Value, (int)rhs.Value).CanonicalForm);
                }
            }

            return FromDouble(Math.Pow(lhs.Value.ToDouble(), rhs.Value.ToDouble()), false, lhs.Form);
        }

        return null;
    }
}