using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions.Binary;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Ast
{
    public class BinaryOperation : Expression {
        public delegate Value OperationFunction(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context);

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

        static BinaryOperation() {
            var ops =
                new (TokenType tokenType, OperationType operationType, OperationFunction function)[] {
                        (TokenType.Add, OperationType.Add, BinaryOperations.Add),
                        (TokenType.Divide, OperationType.Divide, BinaryOperations.Divide),
                        (TokenType.Multiply, OperationType.Multiply, BinaryOperations.Multiply),
                        (TokenType.Subtract, OperationType.Subtract, BinaryOperations.Subtract),
                        (TokenType.Modulus, OperationType.Modulus, BinaryOperations.Modulus),
                        (TokenType.Power, OperationType.Power, BinaryOperations.Power),
                        (TokenType.Equals, OperationType.Equals, BinaryOperations.Equals),
                        (TokenType.NotEquals, OperationType.NotEquals, BinaryOperations.NotEquals),
                        (TokenType.GreaterOrEqualTo, OperationType.GreaterOrEqualTo, BinaryOperations.GreaterOrEqualTo),
                        (TokenType.GreaterThan, OperationType.GreaterThan, BinaryOperations.GreaterThan),
                        (TokenType.LessOrEqualTo, OperationType.LessOrEqualTo, BinaryOperations.LessOrEqualTo),
                        (TokenType.LessThan, OperationType.LessThan, BinaryOperations.LessThan),
                    };

            TypeDictionary = new TwoWayDictionary<TokenType, OperationType>();
            foreach (var op in ops) {
                TypeDictionary.Add(op.tokenType, op.operationType);
            }

            OperationDictionary = ops.ToDictionary(x => x.operationType, x => x.function);
            AllFunctions = ops.Select(x => x.function).ToArray();
        }

        public static readonly OperationFunction[] AllFunctions;

        private static readonly TwoWayDictionary<TokenType, OperationType> TypeDictionary;

        private static readonly Dictionary<OperationType, OperationFunction> OperationDictionary;

        public static OperationType GetTypeFromToken(TokenType token) {
            return TypeDictionary[token];
        }

        public static OperationFunction GetOperationFromType(OperationType type) {
            return OperationDictionary[type];
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
