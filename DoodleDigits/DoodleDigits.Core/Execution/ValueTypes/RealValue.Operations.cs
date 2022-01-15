using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class RealValue {
    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool castAttempt, ExecutionContext<BinaryOperation> context) {
        if (castAttempt && other is IConvertibleToReal convertibleToReal) {
            other = convertibleToReal.ConvertToReal(context, side.Flip());
        }

        if (other is RealValue otherRealValue) {
            return new RealValue(this.Value + otherRealValue.Value, false, Form);
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool castAttempt, ExecutionContext<BinaryOperation> context) {
        if (castAttempt && other is IConvertibleToReal convertibleToReal) {
            other = convertibleToReal.ConvertToReal(context, side.Flip());
        }

        if (other is RealValue otherRealValue) {
            Rational value = side == BinaryOperation.OperationSide.Left ?
                this.Value - otherRealValue.Value : 
                otherRealValue.Value - this.Value;

            return new RealValue(value, false, Form);
        }

        return null;
    }
}