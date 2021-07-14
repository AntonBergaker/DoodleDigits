﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Ast
{
    public class UnaryOperation : Expression {

        public delegate Value OperationFunction(Value value, ExecutionContext<UnaryOperation> context);

        public enum OperationType {
            Add,
            Subtract,
            Factorial,
            Not
        }

        static UnaryOperation() {

            var ops = new (TokenType tokenType, OperationType operationType, OperationFunction function)[] {
                (TokenType.Add, OperationType.Add, UnaryOperations.UnaryPlus),
                (TokenType.Subtract, OperationType.Subtract, UnaryOperations.UnaryNegate),
                (TokenType.Exclamation, OperationType.Not, UnaryOperations.UnaryNot),
            };

            TokenOperationDictionary = ops.ToDictionary(x => x.tokenType, x => x.operationType);

            ops = ops.Append((TokenType.Exclamation, OperationType.Factorial, UnaryOperations.UnaryFactorial)).ToArray();

            OperationTokenDictionary = ops.ToDictionary(x => x.operationType, x => x.tokenType);
            FunctionDictionary = ops.ToDictionary(x => x.operationType, x => x.function);
            AllFunctions = ops.Select(x => x.function).ToArray();
        }

        private static readonly Dictionary<TokenType, OperationType> TokenOperationDictionary;
        private static readonly Dictionary<OperationType, TokenType> OperationTokenDictionary;
        private static readonly Dictionary<OperationType, OperationFunction> FunctionDictionary;

        public static readonly OperationFunction[] AllFunctions;

        public static OperationType GetTypeFromToken(TokenType token) {
            return TokenOperationDictionary[token];
        }

        public static OperationFunction GetFunctionFromType(OperationType type) {
            return FunctionDictionary[type];
        }

        public OperationType Operation { get; }
        public Expression Value { get; }

        public UnaryOperation(OperationType operation, Expression value, Range position) : base(position) {
            Operation = operation;
            Value = value;
        }

        public UnaryOperation(OperationType operation, Expression value) : this(operation, value, 0..0) { }

        public override bool Equals(AstNode other) {
            if (other is not UnaryOperation uo) {
                return false;
            }

            return uo.Operation == Operation && Value.Equals(uo.Value);
        }

        public override Range FullPosition => Utils.Join(Position, Value.FullPosition);

        public override string ToString() {
            if (Operation == OperationType.Factorial) {
                return $"{Value}!";
            }

            return $"{Token.Tokens[OperationTokenDictionary[Operation]]}{Value}";
        }
    }
}
