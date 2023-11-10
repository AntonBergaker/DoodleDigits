using System;
using System.Collections.Generic;
using System.Linq;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing.Ast; 
public static class BinaryOperationExtensions {
    public static BinaryOperation.OperationSide Flip(this BinaryOperation.OperationSide side) {
        return side == BinaryOperation.OperationSide.Left ? BinaryOperation.OperationSide.Right : BinaryOperation.OperationSide.Left;
    }
}

public class BinaryOperation : Expression {
    public delegate Value OperationFunction(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes);
    
    public enum OperationSide {
        Left,
        Right
    }

    public enum OperationType {
        Add,
        Subtract,
        Divide,
        Multiply,
        Modulus,
        Power,
        ShiftLeft,
        ShiftRight,
        BooleanAnd,
        BooleanOr,
        BooleanXor,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        Cross,
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
                    (TokenType.ShiftLeft, OperationType.ShiftLeft, BinaryOperations.ShiftLeft),
                    (TokenType.ShiftRight, OperationType.ShiftRight, BinaryOperations.ShiftRight),
                    (TokenType.BooleanAnd, OperationType.BooleanAnd, BinaryOperations.BooleanAnd),
                    (TokenType.BooleanXor, OperationType.BooleanXor, BinaryOperations.BooleanXor),
                    (TokenType.BooleanOr, OperationType.BooleanOr, BinaryOperations.BooleanOr),
                    (TokenType.BitwiseOr, OperationType.BitwiseOr, BinaryOperations.BitwiseOr),
                    (TokenType.BitwiseXor, OperationType.BitwiseXor, BinaryOperations.Xor),
                    (TokenType.BitwiseAnd, OperationType.BitwiseAnd, BinaryOperations.BitwiseAnd),
                    (TokenType.Cross, OperationType.Cross, BinaryOperations.Cross),
                };

        TypeDictionary = new TwoWayDictionary<TokenType, OperationType>();
        foreach (var (tokenType, operationType, _) in ops) {
            TypeDictionary.Add(tokenType, operationType);
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
