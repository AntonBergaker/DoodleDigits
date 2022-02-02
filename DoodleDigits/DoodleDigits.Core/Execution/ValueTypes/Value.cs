using System;
using System.Diagnostics.CodeAnalysis;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public abstract class Value : IEquatable<Value> {
        public abstract override string ToString();

        public bool TriviallyAchieved { get; }

        internal AstNode? SourceAstNode;

        protected Value(bool triviallyAchieved, AstNode? sourceAstNode) {
            TriviallyAchieved = triviallyAchieved;
            SourceAstNode = sourceAstNode;
        }

        public virtual Value? TryAdd(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }

        public virtual Value? TrySubtract(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }
        public virtual Value? TryMultiply(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }

        public virtual Value? TryDivide(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }

        public virtual Value? TryModulus(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }
        public virtual Value? TryPower(Value other, BinaryOperation.OperationSide side, bool shouldConvert, ExecutionContext<BinaryOperation> context) {
            return null;
        }

        public abstract bool Equals(Value? other);

        public override bool Equals(object? obj) {
            if (obj is not Value val) {
                return false;
            }
            return Equals(val);
        }

        public abstract override int GetHashCode();
        public abstract Value Clone(bool? triviallyAchieved = null);
    }
}
