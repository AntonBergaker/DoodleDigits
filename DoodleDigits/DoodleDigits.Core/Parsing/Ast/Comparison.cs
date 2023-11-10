using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Tokenizing;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing.Ast; 

public class Comparison : Expression {
    public enum ComparisonType {
        Equals,
        NotEquals,
        GreaterOrEqualTo,
        GreaterThan,
        LessOrEqualTo,
        LessThan,
    }

    public delegate Value BinaryEqualsFunction(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes);

    static Comparison() {
        var ops =
            new (TokenType tokenType, ComparisonType operationType, BinaryEqualsFunction function)[] {
                (TokenType.Equals, ComparisonType.Equals, BinaryOperations.Equals),
                (TokenType.NotEquals, ComparisonType.NotEquals, BinaryOperations.NotEquals),
                (TokenType.GreaterOrEqualTo, ComparisonType.GreaterOrEqualTo, BinaryOperations.GreaterOrEqualTo),
                (TokenType.GreaterThan, ComparisonType.GreaterThan, BinaryOperations.GreaterThan),
                (TokenType.LessOrEqualTo, ComparisonType.LessOrEqualTo, BinaryOperations.LessOrEqualTo),
                (TokenType.LessThan, ComparisonType.LessThan, BinaryOperations.LessThan),
            };

        TypeDictionary = new TwoWayDictionary<TokenType, ComparisonType>();
        foreach (var (tokenType, operationType, _) in ops) {
            TypeDictionary.Add(tokenType, operationType);
        }

        OperationDictionary = ops.ToDictionary(x => x.operationType, x => x.function);
        AllFunctions = ops.Select(x => x.function).ToArray();
    }

    public static readonly BinaryEqualsFunction[] AllFunctions;

    private static readonly TwoWayDictionary<TokenType, ComparisonType> TypeDictionary;

    private static readonly Dictionary<ComparisonType, BinaryEqualsFunction> OperationDictionary;


    public class Builder : IEnumerable<Expression> {
        internal Range Position { get; private set; }

        public Builder(Expression lhs) : this(lhs, 0..0) { }

        public Builder(Expression lhs, Range position) {
            Expressions = new List<Expression>() { lhs };
            Signs = new List<ComparisonType>();
            Position = position;
        }

        public Builder Add(ComparisonType sign, Expression rhs) {
            Signs.Add(sign);
            Expressions.Add(rhs);

            return this;
        }

        public Builder Add(ComparisonType sign, Expression rhs, Range position) {
            Position = Utils.Join(Position, position);
            return Add(sign, rhs);
        }

        public Comparison Build() {
            return new Comparison(this);
        }

        public int ExpressionCount => Expressions.Count;

        internal List<Expression> Expressions { get; }
        internal List<ComparisonType> Signs { get; }

        // Only to make the interface implemented so we can use cool add syntax
        public IEnumerator<Expression> GetEnumerator() {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }


    public static ComparisonType GetTypeFromToken(TokenType token) {
        return TypeDictionary[token];
    }

    public static BinaryEqualsFunction GetOperationFromType(ComparisonType type) {
        return OperationDictionary[type];
    }

    public Expression[] Expressions { get; }
    public ComparisonType[] Signs { get; }

    public Comparison(Expression lhs, ComparisonType sign, Expression rhs) : this(new Builder(lhs).Add(sign, rhs)) {
    }

    private Comparison(Builder builder) : base(builder.Position) {
        Expressions = builder.Expressions.ToArray();
        Signs = builder.Signs.ToArray();
    }

    public override bool Equals(AstNode other) {
        if (other is not Comparison otherEqChain) {
            return false;
        }

        if (otherEqChain.Expressions.Length != Expressions.Length) {
            return false;
        }

        for (int i = 0; i < Expressions.Length; i++) {
            if (Expressions[i].Equals(otherEqChain.Expressions[i]) == false) {
                return false;
            }
        }

        for (int i = 0; i < Signs.Length; i++) {
            if (Signs[i] != otherEqChain.Signs[i]) {
                return false;
            }
        }

        return true;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < Expressions.Length; i++) {
            sb.Append(Expressions[i].ToString());
            if (i < Expressions.Length - 1) {
                sb.Append($" {Token.StringForTokenType(TypeDictionary[Signs[i]])} ");
            }
        }

        return sb.ToString();
    }
}
