using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Ast
{
    public class BinaryOperation : Expression {
        public enum OperationType {
            Add,
            Subtract,
            Divide,
            Multiply,
            Modulus,
            Power,
            Equals,
            NotEquals,
            GreaterOrEqualTo,
            GreaterThan,
            LessOrEqualTo,
            LessThan
        }


        private static readonly TwoWayDictionary<TokenType, OperationType> TypeDictionary = new() {
            {TokenType.Add, OperationType.Add},
            {TokenType.Divide, OperationType.Divide},
            {TokenType.Multiply, OperationType.Multiply},
            {TokenType.Subtract, OperationType.Subtract},
            {TokenType.Modulus, OperationType.Modulus},
            {TokenType.Power, OperationType.Power},
            {TokenType.Equals, OperationType.Equals},
            {TokenType.NotEquals, OperationType.NotEquals},
            {TokenType.GreaterOrEqualTo, OperationType.GreaterOrEqualTo},
            {TokenType.GreaterThan, OperationType.GreaterThan},
            {TokenType.LessOrEqualTo, OperationType.LessOrEqualTo},
            {TokenType.LessThan, OperationType.LessThan},
        };

        public static OperationType GetTypeFromToken(TokenType token) {
            return TypeDictionary[token];
        }


        public Expression Left { get; }
        public OperationType Operation { get; }
        public Expression Right { get; }

        public BinaryOperation(Expression left, OperationType operation, Expression right, Range position) : base(position) {
            Left = left;
            Operation = operation;
            Right = right;
        }

        public BinaryOperation(Expression left, OperationType operation, Expression right) : this(left, operation, right, 0..0) {
        }

        public override bool Equals(AstNode other) {
            if (other is not BinaryOperation bo) {
                return false;
            }

            return Operation == bo.Operation && Left.Equals(bo.Left) && Right.Equals(bo.Right);
        }

        public override string ToString() {
            return $"({Left} {Token.Tokens[TypeDictionary[Operation]]} {Right})";
        }

    }
}
