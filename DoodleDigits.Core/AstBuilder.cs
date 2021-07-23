﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using DoodleDigits.Core.Ast;

namespace DoodleDigits.Core
{
    public class AstResult {
        public readonly List<Error> Errors;
        public readonly AstNode Root;

        public AstResult(List<Error> errors, AstNode root) {
            Errors = errors;
            Root = root;
        }
    }

    public class AstBuilder {
        private readonly HashSet<string> functionNames;
        private readonly Tokenizer tokenizer;
        private TokenReader reader;
        private readonly List<Error> errors;

        public AstBuilder(IEnumerable<string> functionNames) {
            this.functionNames = functionNames.ToHashSet();
            tokenizer = new Tokenizer();
            reader = null!;
            errors = new List<Error>();
        }


        public AstResult Build(string input) {
            errors.Clear();
            reader = new TokenReader(tokenizer.Tokenize(input));

            tokenizer.Tokenize(input);

            AstNode statements = ReadStatements();

            return new AstResult(new List<Error>(errors), statements);
        }

        private AstNode ReadStatements() {
            List<Expression> expressions = new();
            while (reader.ReachedEnd == false) {
                Expression expression = ReadExpression();
                if (expression is not ErrorNode) {
                    expressions.Add(expression);
                }

                Token peek = reader.Peek();
                if (peek.Type == TokenType.Comma) {
                    reader.Skip();
                }

            }

            if (expressions.Count == 1) {
                return expressions[0];
            }

            return new ExpressionList(expressions, 0..0);
        }

        private Expression ReadExpression() {
            return ReadPreEqualsBinary();
        }

        private Expression ReadEquals() {
            Func<Expression> next = ReadImplicitMultiplication;

            Expression lhs = next();

            Token nextToken = reader.Peek(false);
            if (nextToken.Type is TokenType.Equals or TokenType.NotEquals) {
                EqualsComparison.Builder builder = new(lhs, nextToken.Position);

                while (nextToken.Type is TokenType.Equals or TokenType.NotEquals) {
                    reader.Skip(false);
                    Expression rhs = next();
                    if (rhs is ErrorNode) {
                        break;
                    }

                    builder.Add(EqualsComparison.GetTypeFromToken(nextToken.Type), rhs, nextToken.Position);
                    nextToken = reader.Peek();
                }

                return builder.Build();
            }

            return lhs;
        }

        private Expression ReadImplicitMultiplication() {
            Expression lhs = ReadPostEqualsBinary();

            Token peek = reader.Peek(false);
            while (peek.Type is TokenType.ParenthesisOpen or TokenType.Identifier or TokenType.Number) {
                lhs = new BinaryOperation(lhs, BinaryOperation.OperationType.Multiply,
                    ReadPostEqualsBinary(), reader.Position..reader.Position);
                peek = reader.Peek();
            }

            return lhs;
        }


        private static readonly TokenType[][] preEqualsBinaryOperationOrder = new[] {
            new[] {TokenType.BooleanAnd},
        };
        private Expression ReadPreEqualsBinary() {
            return ReadBinary(preEqualsBinaryOperationOrder, preEqualsBinaryOperationOrder.Length-1, ReadEquals);
        }

        private static readonly TokenType[][] postEqualsBinaryOperationOrder = new[] {
            new[] {TokenType.Power},
            new[] {TokenType.Multiply, TokenType.Divide, TokenType.Modulus},
            new[] {TokenType.Add, TokenType.Subtract},
            new[] {TokenType.ShiftLeft, TokenType.ShiftRight},
            new[] {TokenType.GreaterOrEqualTo, TokenType.GreaterThan, TokenType.LessThan, TokenType.LessOrEqualTo},
        };

        private Expression ReadPostEqualsBinary() {
            return ReadBinary(postEqualsBinaryOperationOrder, postEqualsBinaryOperationOrder.Length - 1, ReadPreUnary);
        }


        private Expression ReadBinary(TokenType[][] operations, int depth, Func<Expression> baseNext) {
            if (depth == -1) {
                return baseNext();
            }
            Expression Next() => this.ReadBinary(operations, depth - 1, baseNext);

            Expression lhs = Next();

            TokenType[] operatorsTop = operations[depth];

            Token nextToken = reader.Peek(false);
            while (operatorsTop.Contains(nextToken.Type)) {
                reader.Skip(false);
                Expression rhs = Next();
                if (rhs is ErrorNode) {
                    break;
                }
                lhs = new BinaryOperation(lhs, BinaryOperation.GetTypeFromToken(nextToken.Type), rhs, nextToken.Position);
                nextToken = reader.Peek(false);
            }

            return lhs;
        }


        private Expression ReadPreUnary() {

            Token peek = reader.Peek();

            if (peek.Type is TokenType.Add or TokenType.Subtract or TokenType.Exclamation) {
                reader.Skip();
                return new UnaryOperation(UnaryOperation.GetTypeFromToken(peek.Type), ReadPreUnary(), peek.Position);
            }

            return ReadPostUnary();
        }

        private Expression ReadPostUnary() {
            Expression expression = ReadLiteral();

            Token peek = reader.Peek();
            while (peek.Type == TokenType.Exclamation) {
                reader.Skip();
                expression = new UnaryOperation(UnaryOperation.OperationType.Factorial, expression, peek.Position);
                peek = reader.Peek();
            }

            return expression;
        }

        private Expression ReadLiteral() {
            return ReadLiteral(reader.Read());
        }

        private Expression ReadLiteral(Token token) {

            switch (token.Type) {
                case TokenType.ParenthesisOpen:
                    Expression expression = ReadExpression();
                    // This should be a parenthesis
                    Token nextToken = reader.Read();
                    if (nextToken.Type != TokenType.ParenthesisClose) {
                        errors.Add(new Error(nextToken.Position, "Unclosed parenthesis"));
                    }

                    return expression;
                case TokenType.Number:
                    return new NumberLiteral(token.Content, token.Position);
                case TokenType.Identifier:
                    return ReadIdentifier(token);
            }

            return new ErrorNode();
        }


        private Expression ReadIdentifier(Token token) {
            if (token.Content.StartsWith("log")) {
                if (ReadFunctionWithBuiltInParameter(token, "log", out Expression? log)) {
                    return log;
                }
            }
            if (token.Content.StartsWith("root")) {
                if (ReadFunctionWithBuiltInParameter(token, "root", out Expression? root)) {
                    return root;
                }
            }

            if (functionNames.Contains(token.Content.ToLower())) {
                return ReadFunction(token);
            }

            return new Identifier(token.Content, token.Position);
        }

        private Expression ReadFunction(Token token) {
            Token next = reader.Peek();
            if (next.Type == TokenType.ParenthesisOpen) {
                List<Expression> parameters = new();
                reader.Skip();

                while (reader.ReachedEnd == false) {
                    Expression ex = ReadExpression();
                    if (ex is ErrorNode) {
                        break;
                    }

                    parameters.Add(ex);

                    Token peek = reader.Peek();
                    if (peek.Type is TokenType.ParenthesisClose or TokenType.EndOfFile) {
                        reader.Skip();
                        break;
                    }

                    if (peek.Type == TokenType.Comma) {
                        reader.Skip();
                        continue;
                    }

                }

                return new Function(token.Content, parameters, token.Position);
            }

            Expression expression = ReadExpression();
            return new Function(token.Content, new[] { expression }, token.Position);
        }

        private bool ReadFunctionWithBuiltInParameter(Token token, string functionName, [NotNullWhen(true)] out Expression? function) {
            function = null;
            int functionLength = functionName.Length;

            if (token.Content.Length == functionLength) {
                // Log will be handled by the normal mechanisms under function when false
                return false;
            }

            if (token.Content[functionLength] == '_') {
                // Underscore means we try to read a literal down here
                // Parse the one token
                Token? hotSwappedToken = tokenizer.TokenizeOne(token.Content, functionLength+1);
                if (hotSwappedToken == null || hotSwappedToken.Type is TokenType.Unknown or TokenType.EndOfFile) {
                    return false;
                }

                Expression @base = ReadLiteral(hotSwappedToken);
                Expression argument = ReadExpression();

                function = new Function(functionName, new[] {argument, @base}, token.Position);
                return true;
            }

            if (char.IsDigit(token.Content[functionLength])) {
                if (double.TryParse(token.Content[functionLength..], out double @base)) {
                    Range newRange = (token.Position.Start.Value + functionLength)..token.Position.End;
                    Expression baseLiteral = new NumberLiteral(@base.ToString(CultureInfo.InvariantCulture), newRange);
                    Expression argument = ReadExpression();

                    function = new Function(functionName, new[] { argument, baseLiteral }, token.Position);
                    return true;
                }
            }

            return false;
        }
    }
}
