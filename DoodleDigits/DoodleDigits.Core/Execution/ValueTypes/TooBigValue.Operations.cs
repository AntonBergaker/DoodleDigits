using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;

public partial class TooBigValue {
    public override Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool castAttempt,
        ExecutionContext<BinaryOperation> context) {
        if (other is TooBigValue otherTbv) {
            if (this.ValueSign is Sign.Positive or Sign.Negative &&
                otherTbv.ValueSign is Sign.NegativeInfinity or Sign.PositiveInfinity) {
                return otherTbv;
            }

            return this;
        }

        if (castAttempt && other is IConvertibleToReal otherCtr) {
            // Anything we convert to is smaller than this so just return this
            return this;
        }

        return null;
    }

    public override Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool castAttempt,
        ExecutionContext<BinaryOperation> context) {
        if (other is TooBigValue otherTbv) {
            if (this.ValueSign is Sign.Positive or Sign.Negative &&
                otherTbv.ValueSign is Sign.NegativeInfinity or Sign.PositiveInfinity) {
                return otherTbv;
            }

            return this;
        }

        if (castAttempt && other is IConvertibleToReal otherCtr) {
            // Anything we convert to is smaller than this so just return this

            // Negative if we're on the right side
            if (side == BinaryOperation.OperationSide.Right) {
                return this.Negate();
            }

            return this;
        }

        return null;
    }
}