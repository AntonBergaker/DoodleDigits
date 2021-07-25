﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions.Binary;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Ast
{
    public class BinaryOperation : Expression {
        public delegate Value OperationFunction(Value lhs, Value rhs, ExecutionContext<BinaryOperation> context);

        public override Range FullPosition => Utils.Join(Position, Lhs.FullPosition, Rhs.FullPosition);

        public enum OperationType {
            Add,
            Subtract,
            Divide,
            Multiply,
            Modulus,
            Power,
            GreaterOrEqualTo,
            GreaterThan,
            LessOrEqualTo,
            LessThan,
            ShiftLeft,
            ShiftRight,
            BooleanAnd,
            BooleanOr,
            BooleanXor,
            BitwiseAnd,
            BitwiseOr,
            BitwiseXor,
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
                        (TokenType.GreaterOrEqualTo, OperationType.GreaterOrEqualTo, BinaryOperations.GreaterOrEqualTo),
                        (TokenType.GreaterThan, OperationType.GreaterThan, BinaryOperations.GreaterThan),
                        (TokenType.LessOrEqualTo, OperationType.LessOrEqualTo, BinaryOperations.LessOrEqualTo),
                        (TokenType.LessThan, OperationType.LessThan, BinaryOperations.LessThan),
                        (TokenType.ShiftLeft, OperationType.ShiftLeft, BinaryOperations.ShiftLeft),
                        (TokenType.ShiftRight, OperationType.ShiftRight, BinaryOperations.ShiftRight),
                        (TokenType.BooleanAnd, OperationType.BooleanAnd, BinaryOperations.BooleanAnd),
                        (TokenType.BooleanXor, OperationType.BooleanXor, BinaryOperations.BooleanXor),
                        (TokenType.BooleanOr, OperationType.BooleanOr, BinaryOperations.BooleanOr),
                        (TokenType.BitwiseOr, OperationType.BitwiseOr, BinaryOperations.BitwiseAnd),
                        (TokenType.BitwiseXor, OperationType.BitwiseXor, BinaryOperations.Xor),
                        (TokenType.BitwiseAnd, OperationType.BitwiseAnd, BinaryOperations.BitwiseOr),
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


        public Expression Lhs { get; }
        public OperationType Operation { get; }
        public Expression Rhs { get; }

        public BinaryOperation(Expression lhs, OperationType operation, Expression rhs, Range position) : base(position) {
            Lhs = lhs;
            Operation = operation;
            Rhs = rhs;
        }

        public BinaryOperation(Expression lhs, OperationType operation, Expression rhs) : this(lhs, operation, rhs, 0..0) {
        }

        public override bool Equals(AstNode other) {
            if (other is not BinaryOperation bo) {
                return false;
            }

            return Operation == bo.Operation && Lhs.Equals(bo.Lhs) && Rhs.Equals(bo.Rhs);
        }

        public override string ToString() {
            return $"({Lhs} {Token.StringForTokenType(TypeDictionary[Operation])} {Rhs})";
        }

    }
}
