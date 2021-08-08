using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Tokenizing;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing
{
    public class ParseResult {
        public readonly List<ParseError> Errors;
        public readonly AstNode Root;

        public ParseResult(List<ParseError> errors, AstNode root) {
            Errors = errors;
            Root = root;
        }
    }

    public class Parser {
        private readonly HashSet<string> functionNames;
        private readonly Tokenizer tokenizer;
        private TokenReader reader;
        private readonly List<ParseError> errors;

        private bool insideAbsoluteExpression;

        public Parser(IEnumerable<string> functionNames) {
            this.functionNames = functionNames.ToHashSet();
            tokenizer = new Tokenizer();
            reader = null!;
            errors = new List<ParseError>();
        }


        public ParseResult Parse(string input) {
            errors.Clear();
            insideAbsoluteExpression = false;

            reader = new TokenReader(tokenizer.Tokenize(input));

            tokenizer.Tokenize(input);

            AstNode statements = ReadStatements();

            return new ParseResult(new List<ParseError>(errors), statements);
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

            if (expressions.Count == 0) {
                return new ExpressionList(expressions, 0..0);
            }

            return new ExpressionList(expressions, Utils.Join(expressions.Select(x => x.Position).ToArray()));
        }

        private Expression ReadExpression() {
            return ReadBinaryBooleanOr();
        }

        private Expression ReadBinaryBooleanOr() => GenericReadBinary(TokenType.BooleanOr, ReadBinaryBooleanXor);
        private Expression ReadBinaryBooleanXor() => GenericReadBinary(TokenType.BooleanXor, ReadBinaryBooleanAnd);
        private Expression ReadBinaryBooleanAnd() => GenericReadBinary(TokenType.BooleanAnd, ReadBinaryBitwiseOr);
        private Expression ReadBinaryBitwiseOr() => GenericReadBinary(TokenType.BooleanOr, ReadBinaryBitwiseXor);
        private Expression ReadBinaryBitwiseXor() => GenericReadBinary(TokenType.BooleanXor, ReadBinaryBitwiseAnd);
        private Expression ReadBinaryBitwiseAnd() => GenericReadBinary(TokenType.BooleanAnd, ReadComparison);

        private static readonly TokenType[] ComparisonTokens = new[] {
            TokenType.Equals, TokenType.NotEquals, TokenType.GreaterOrEqualTo, TokenType.GreaterThan,
            TokenType.LessThan, TokenType.LessOrEqualTo
        };
        private Expression ReadComparison() {
            Func<Expression> next = ReadBinaryShifting;

            Expression lhs = next();

            Token nextToken = reader.Peek(false);
            if (ComparisonTokens.Contains(nextToken.Type)) {
                Comparison.Builder builder = new(lhs, Utils.Join(nextToken.Position, lhs.Position));

                while (ComparisonTokens.Contains(nextToken.Type)) {
                    reader.Skip(false);
                    Expression rhs = next();
                    if (rhs is ErrorNode) {
                        break;
                    }

                    builder.Add(Comparison.GetTypeFromToken(nextToken.Type), rhs, Utils.Join(nextToken.Position, rhs.Position));
                    nextToken = reader.Peek();
                }

                return builder.Build();
            }

            return lhs;
        }

        private Expression ReadBinaryShifting() => GenericReadBinary(new[] { TokenType.ShiftLeft, TokenType.ShiftRight }, ReadBinaryAddSubtract);
        private Expression ReadBinaryAddSubtract() => GenericReadBinary(new[] { TokenType.Add, TokenType.Subtract }, ReadMultiplyDivide);
        private Expression ReadMultiplyDivide() => GenericReadBinary(new[] { TokenType.Multiply, TokenType.Divide, TokenType.Modulus }, ReadImplicitMultiplication);

        private Expression ReadImplicitMultiplication() {
            Expression lhs = ReadPower();

            Token peek = reader.Peek(false);
            while (peek.Type is TokenType.ParenthesisOpen or TokenType.Identifier or TokenType.Number ||
                   (peek.Type == TokenType.AbsoluteLine && insideAbsoluteExpression == false)) {
                Expression rhs = ReadPower();
                if (rhs is ErrorNode) {
                    break;
                }
                lhs = new BinaryOperation(lhs, BinaryOperation.OperationType.Multiply, rhs,
                    Utils.Join(lhs.Position, rhs.Position));
                peek = reader.Peek(false);
            }

            return lhs;
        }
        
        private Expression ReadPower() => GenericReadBinary(TokenType.Power, ReadPreUnary);

        private Expression GenericReadBinary(TokenType operation, Func<Expression> next) {
            return GenericReadBinary(new[] {operation}, next);
        }

        private Expression GenericReadBinary(TokenType[] operations, Func<Expression> next) {
            Expression lhs = next();

            Token nextToken = reader.Peek(false);
            while (operations.Contains(nextToken.Type)) {
                reader.Skip(false);
                Expression rhs = next();
                if (rhs is ErrorNode) {
                    break;
                }
                lhs = new BinaryOperation(lhs, BinaryOperation.GetTypeFromToken(nextToken.Type), rhs, Utils.Join(lhs.Position, nextToken.Position, rhs.Position));
                nextToken = reader.Peek(false);
            }

            return lhs;
        }


        private Expression ReadPreUnary() {

            Token peek = reader.Peek();

            if (peek.Type is TokenType.Add or TokenType.Subtract or TokenType.Exclamation) {
                reader.Skip();
                Expression value = ReadPreUnary();
                return new UnaryOperation(UnaryOperation.GetTypeFromToken(peek.Type), value, Utils.Join(peek.Position, value.Position));
            }

            return ReadPostUnary();
        }

        private Expression ReadPostUnary() {
            Expression expression = ReadLiteral();

            Token peek = reader.Peek();
            while (peek.Type == TokenType.Exclamation) {
                reader.Skip();
                expression = new UnaryOperation(UnaryOperation.OperationType.Factorial, expression, Utils.Join(expression.Position, peek.Position));
                peek = reader.Peek();
            }

            return expression;
        }

        private Expression ReadLiteral() {
            return ReadLiteral(reader.Read());
        }

        private Expression ReadLiteral(Token token) {
            return token.Type switch {
                TokenType.AbsoluteLine => ReadAbsoluteExpression(token),
                TokenType.ParenthesisOpen => ReadParenthesis(token),
                TokenType.Number => new NumberLiteral(token.Content, token.Position),
                TokenType.Identifier => ReadIdentifier(token),
                _ => new ErrorNode()
            };
        }

        // Assumes token is a |
        private Expression ReadAbsoluteExpression(Token token) {
            // Flag as being inside an absolute expression, and save the previous state
            bool wasInsideAbsolute = insideAbsoluteExpression;
            insideAbsoluteExpression = true;

            Expression expression = ReadExpression();

            insideAbsoluteExpression = wasInsideAbsolute;

            // This should be a |
            Token nextToken = reader.Read();
            Index end = nextToken.Position.End;
            if (nextToken.Type != TokenType.AbsoluteLine) {
                errors.Add(new ParseError(nextToken.Position, "Unclosed absolute line"));
                end = expression.Position.End;
            }

            return new Function("abs", new[] { expression}, token.Position.Start..end);
        }


        private Expression ReadParenthesis(Token token) {
            // Flag as no longer being inside an absolute expression, as we need the closed parenthesis to close the absolute
            bool wasInsideAbsolute = insideAbsoluteExpression;
            insideAbsoluteExpression = false;

            Expression expression = ReadExpression();

            insideAbsoluteExpression = wasInsideAbsolute;

            // This should be a parenthesis
            Token nextToken = reader.Read();
            Index end = nextToken.Position.End;
            if (nextToken.Type != TokenType.ParenthesisClose) {
                errors.Add(new ParseError(nextToken.Position, "Unclosed parenthesis"));
                end = expression.Position.End;
            }



            expression.Position = token.Position.Start..end;
            return expression;
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
            Index start = token.Position.Start;
            Index end = token.Position.End;
            if (next.Type == TokenType.ParenthesisOpen) {
                // Flag as no longer being inside an absolute expression, as we need the closed parenthesis to close the absolute
                bool wasInsideAbsolute = insideAbsoluteExpression;
                insideAbsoluteExpression = false;

                List<Expression> parameters = new();
                reader.Skip();

                while (reader.ReachedEnd == false) {
                    Expression ex = ReadExpression();
                    if (ex is ErrorNode) {
                        break;
                    }

                    parameters.Add(ex);
                    end = ex.Position.End;

                    Token peek = reader.Peek();
                    if (peek.Type is TokenType.ParenthesisClose or TokenType.EndOfFile) {
                        reader.Skip();
                        end = peek.Position.End;

                        // Go back to previous state
                        insideAbsoluteExpression = wasInsideAbsolute;
                        break;
                    }

                    if (peek.Type == TokenType.Comma) {
                        reader.Skip();
                        end = peek.Position.End;
                        continue;
                    }

                }

                return new Function(token.Content, parameters, start..end);
            }

            Expression expression = ReadImplicitMultiplication();
            end = expression.Position.End;
            return new Function(token.Content, new[] { expression }, start..end);
        }

        private bool ReadFunctionWithBuiltInParameter(Token token, string functionName, [NotNullWhen(true)] out Expression? function) {
            function = null;
            int functionLength = functionName.Length;

            if (token.Content.Length == functionLength) {
                // The function will be handled by the normal mechanisms under function when false
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
                Expression argument = ReadImplicitMultiplication();

                function = new Function(functionName, new[] {argument, @base}, Utils.Join(token.Position, argument.Position));
                return true;
            }

            if (char.IsDigit(token.Content[functionLength])) {
                if (double.TryParse(token.Content[functionLength..], out double @base)) {
                    Range newRange = (token.Position.Start.Value + functionLength)..token.Position.End;
                    Expression baseLiteral = new NumberLiteral(@base.ToString(CultureInfo.InvariantCulture), newRange);
                    Expression argument = ReadLiteral();

                    function = new Function(functionName, new[] { argument, baseLiteral }, Utils.Join(token.Position, argument.Position));
                    return true;
                }
            }

            return false;
        }
    }
}
