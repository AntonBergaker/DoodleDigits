﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;
using DoodleDigits.Core.Tokenizing;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing;

public class ParseResult {
    public readonly List<ParseError> Errors;
    public readonly AstNode Root;

    public ParseResult(List<ParseError> errors, AstNode root) {
        Errors = errors;
        Root = root;
    }
}

public class Parser {
    private readonly Dictionary<string, FunctionData> _functions;
    private readonly Tokenizer _tokenizer;
    private TokenReader _reader;
    private readonly List<ParseError> _errors;

    private bool _insideAbsoluteExpression;

    public Parser(IEnumerable<FunctionData> functions) { 
        this._functions = new();
        foreach (FunctionData function in functions) {
            foreach (string functionName in function.Names) {
                this._functions.Add(functionName, function);
            }
        }
        _tokenizer = new Tokenizer();
        _reader = null!;
        _errors = new List<ParseError>();
    }


    public ParseResult Parse(string input) {
        _errors.Clear();
        _insideAbsoluteExpression = false;

        _reader = new TokenReader(_tokenizer.Tokenize(input));

        _tokenizer.Tokenize(input);

        AstNode statements = ReadStatements();

        return new ParseResult(new List<ParseError>(_errors), statements);
    }

    private AstNode ReadStatements() {
        List<AstNode> nodes = new();
        while (_reader.ReachedEnd == false) {
            AstNode expression = ReadNode();
            if (expression is not ErrorNode) {
                nodes.Add(expression);
            }

            Token peek = _reader.Peek();
            if (peek.Type == TokenType.Comma) {
                _reader.Skip();
            }

        }

        if (nodes.Count == 1) {
            return nodes[0];
        }

        if (nodes.Count == 0) {
            return new NodeList(nodes, 0..0);
        }

        return new NodeList(nodes, Utils.Join(nodes.Select(x => x.Position).ToArray()));
    }

    private AstNode ReadNode() {
        // Check for function declaration
        if (_reader.Peek().Type == TokenType.Identifier &&
            _reader.Peek(1).Type == TokenType.ParenthesisOpen) {
            return ReadFunctionDeclaration();
        }

        return ReadExpression();
    }

    private AstNode ReadFunctionDeclaration() {
        var expression = ReadExpression();

        // Must be a comparison with a single =
        if (expression is not Comparison comparison) {
            return expression;
        }
        if (comparison.Expressions.Length != 2 || comparison.Signs[0] != Comparison.ComparisonType.Equals) {
            return expression;
        }

        string[] argumentNames;
        var lhs = comparison.Expressions[0];
        // Lhs must be a function call or multiplication since that matches the x(x) syntax
        if (lhs is not FunctionCallOrMultiplication functionCall) {
            return expression;
        }

        if (functionCall.Multiplication.Lhs is not Identifier identifier) {
            return expression;
        }
        var binaryRhs = functionCall.Multiplication.Rhs;

        if (binaryRhs is Identifier rhsIdentifier) {
            argumentNames = [rhsIdentifier.Value];
        } else if (binaryRhs is VectorDeclaration rhsVector) {
            if (rhsVector.Expressions.All(x => x is Identifier) == false) {
                return expression;
            }

            argumentNames = rhsVector.Expressions.Cast<Identifier>().Select(x => x.Value).ToArray();
        } else {
            return expression;
        }

        // Rhs is just a funny expression
        var rhs = comparison.Expressions[1];

        return new FunctionDeclarationOrEquals(
            new FunctionDeclaration(
                identifier.Value,
                argumentNames,
                rhs
            ), comparison, expression.Position
        );
    }

    private Expression ReadExpression() {
        return ReadBaseCast();
    }

    private Expression ReadBaseCast() {
        Expression expression = ReadBinaryBooleanOr();

        if (_reader.Peek().Type is TokenType.As or TokenType.In) {
            _reader.Skip();

            Token type = _reader.Peek();

            if (type.Type != TokenType.Identifier) {
                _errors.Add(new ParseError(type.Position, $"{type.Content} is not a base identifier"));
                return expression;
            }
            _reader.Skip();

            return new BaseCast(expression, BaseCast.StringToTarget(type.Content), expression.Position.Start..type.Position.End);
        }

        return expression;
    }

    private Expression ReadBinaryBooleanOr() => GenericReadBinary(TokenType.BooleanOr, ReadBinaryBooleanXor);
    private Expression ReadBinaryBooleanXor() => GenericReadBinary(TokenType.BooleanXor, ReadBinaryBooleanAnd);
    private Expression ReadBinaryBooleanAnd() => GenericReadBinary(TokenType.BooleanAnd, ReadBinaryBitwiseOr);
    private Expression ReadBinaryBitwiseOr() => GenericReadBinary(TokenType.BitwiseOr, ReadBinaryBitwiseXor);
    private Expression ReadBinaryBitwiseXor() => GenericReadBinary(TokenType.BitwiseXor, ReadBinaryBitwiseAnd);
    private Expression ReadBinaryBitwiseAnd() => GenericReadBinary(TokenType.BitwiseAnd, ReadComparison);


    private static readonly TokenType[] ComparisonTokens = [
        TokenType.Equals, TokenType.NotEquals, TokenType.GreaterOrEqualTo, TokenType.GreaterThan,
        TokenType.LessThan, TokenType.LessOrEqualTo
    ];
    private Expression ReadComparison() {
        Func<Expression> next = ReadBinaryShifting;

        Expression lhs = next();

        Token nextToken = _reader.Peek(false);
        if (ComparisonTokens.Contains(nextToken.Type)) {
            Comparison.Builder builder = new(lhs, Utils.Join(nextToken.Position, lhs.Position));

            while (ComparisonTokens.Contains(nextToken.Type)) {
                _reader.Skip(false);
                Expression rhs = next();
                if (rhs is ErrorNode) {
                    break;
                }

                builder.Add(Comparison.GetTypeFromToken(nextToken.Type), rhs, Utils.Join(nextToken.Position, rhs.Position));
                nextToken = _reader.Peek();
            }

            // If there's only a single argument, return as it is
            if (builder.ExpressionCount <= 1) {
                return lhs;
            }

            return builder.Build();
        }

        return lhs;
    }

    private Expression ReadBinaryShifting() => GenericReadBinary(new[] { TokenType.ShiftLeft, TokenType.ShiftRight }, ReadBinaryAddSubtract);
    private Expression ReadBinaryAddSubtract() => GenericReadBinary(new[] { TokenType.Add, TokenType.Subtract }, ReadMultiplyDivide);

    private bool CanBeImplicitlyMultiplied(TokenType type) {
        return type is TokenType.ParenthesisOpen or TokenType.Identifier or TokenType.Number or TokenType.BracketOpen ||
               (type == TokenType.AbsoluteLine && _insideAbsoluteExpression == false);
    }

    private Expression ReadMultiplyDivide() {
        Func<Expression> next = ReadPower;
        Expression lhs = next();

        Token peek = _reader.Peek(false);
        while (true) {
            BinaryOperation.OperationType? type = null;
            // Implicit multiplication
            if (CanBeImplicitlyMultiplied(peek.Type)) {
                type = BinaryOperation.OperationType.Multiply;
            }
            // Explicit multiplication or division
            if (peek.Type is TokenType.Multiply or TokenType.Divide or TokenType.Modulus or TokenType.Cross) {
                _reader.Skip(false);
                type = BinaryOperation.GetTypeFromToken(peek.Type);
            }

            if (type != null) {
                Expression rhs = next();
                if (rhs is ErrorNode) {
                    break;
                }
                lhs = new BinaryOperation(lhs, type.Value, rhs,
                    Utils.Join(lhs.Position, rhs.Position));
                peek = _reader.Peek(false);
                continue;
            }
            else {
                break;
            }
        }

        return lhs;
    }

    private Expression ReadOnlyImplicitMultiplication() {
        Func<Expression> next = ReadPower;
        Expression lhs = next();

        Token peek = _reader.Peek(false);
        while (CanBeImplicitlyMultiplied(peek.Type)) {
            Expression rhs = next();
            if (rhs is ErrorNode) {
                break;
            }
            lhs = new BinaryOperation(lhs, BinaryOperation.OperationType.Multiply, rhs,
                Utils.Join(lhs.Position, rhs.Position));
            peek = _reader.Peek(false);
        }

        return lhs;
    }
   
    private Expression ReadPower() => GenericReadBinary(TokenType.Power, ReadPreUnary);

    private Expression GenericReadBinary(TokenType operation, Func<Expression> next) {
        return GenericReadBinary(new[] {operation}, next);
    }

    private Expression GenericReadBinary(TokenType[] operations, Func<Expression> next) {
        Expression lhs = next();

        Token nextToken = _reader.Peek(false);
        while (operations.Contains(nextToken.Type)) {
            _reader.Skip(false);
            Expression rhs = next();
            if (rhs is ErrorNode) {
                break;
            }
            lhs = new BinaryOperation(lhs, BinaryOperation.GetTypeFromToken(nextToken.Type), rhs, Utils.Join(lhs.Position, nextToken.Position, rhs.Position));
            nextToken = _reader.Peek(false);
        }

        return lhs;
    }


    private Expression ReadPreUnary() {

        Token peek = _reader.Peek();

        if (peek.Type is TokenType.Add or TokenType.Subtract or TokenType.Exclamation) {
            _reader.Skip();
            Expression value = ReadPreUnary();
            return new UnaryOperation(UnaryOperation.GetTypeFromToken(peek.Type), value, Utils.Join(peek.Position, value.Position));
        }

        return ReadPostUnary();
    }

    private Expression ReadPostUnary() {
        Expression expression = ReadLiteral();

        Token peek = _reader.Peek();
        while (peek.Type == TokenType.Exclamation) {
            _reader.Skip();
            expression = new UnaryOperation(UnaryOperation.OperationType.Factorial, expression, Utils.Join(expression.Position, peek.Position));
            peek = _reader.Peek();
        }

        return expression;
    }

    private Expression ReadLiteral() {
        var token = _reader.Read();
        return token.Type switch {
            TokenType.AbsoluteLine => ReadAbsoluteExpression(token),
            TokenType.ParenthesisOpen => ReadVectorOrParenthesis(token),
            TokenType.BracketOpen => ReadVectorOrParenthesis(token),
            TokenType.Number => new NumberLiteral(token.Content, token.Position),
            TokenType.Identifier => ReadFunction(token),
            _ => new ErrorNode()
        };
    }

    private Expression GetLiteralOrIdentifier(Token token) {
        return token.Type switch {
            TokenType.Number => new NumberLiteral(token.Content, token.Position),
            TokenType.Identifier => ReadIdentifier(token),
            _ => new ErrorNode()
        };
    }

    // Assumes token is a |
    private Expression ReadAbsoluteExpression(Token token) {
        // Flag as being inside an absolute expression, and save the previous state
        bool wasInsideAbsolute = _insideAbsoluteExpression;
        _insideAbsoluteExpression = true;

        Expression expression = ReadExpression();

        _insideAbsoluteExpression = wasInsideAbsolute;

        // This should be a |
        Token nextToken = _reader.Read();
        Index end = nextToken.Position.End;
        if (nextToken.Type != TokenType.AbsoluteLine) {
            _errors.Add(new ParseError(nextToken.Position, "Unclosed absolute line"));
            end = expression.Position.End;
        }

        return new FunctionCall("abs", new[] { expression}, token.Position.Start..end);
    }


    private Expression ReadVectorOrParenthesis(Token token) {
        // Flag as no longer being inside an absolute expression, as we need the closed parenthesis to close the absolute
        bool wasInsideAbsolute = _insideAbsoluteExpression;
        _insideAbsoluteExpression = false;

        Expression expression = ReadExpression();


        // This should be a parenthesis if parenthesis, or a comma if a vector
        Token nextToken = _reader.Peek();
        TokenType expectedEnd = token.Type == TokenType.BracketOpen ? TokenType.BracketClose : TokenType.ParenthesisClose;
        if (nextToken.Type == expectedEnd) {
            _reader.Skip();
            expression.Position = token.Position.Start..nextToken.Position.End;
            _insideAbsoluteExpression = wasInsideAbsolute;
            return expression;
        }

        // If vectorlike
        if (nextToken.Type == TokenType.Comma) {
            List<Expression> expressions = new() { expression };
            while (nextToken.Type == TokenType.Comma) {
                _reader.Skip();
                Expression nextExpression = ReadExpression();
                if (nextExpression is not ErrorNode) {
                    expressions.Add(nextExpression);
                }
                nextToken = _reader.Peek();
            }

            expression = new VectorDeclaration(expressions, token.Position.Start..expressions.Last().Position.End);

            if (nextToken.Type == expectedEnd) {
                _reader.Skip();
                expression.Position = token.Position.Start..nextToken.Position.End;
                _insideAbsoluteExpression = wasInsideAbsolute;
                return expression;
            }
        }

        // If neither, flag as unclosed parenthesis and return
        _errors.Add(new ParseError(nextToken.Position, "Unclosed parenthesis"));
        expression.Position = token.Position.Start..expression.Position.End;
        _insideAbsoluteExpression = wasInsideAbsolute;
        return expression;
    }

    private Expression ReadFunction(Token token) {
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

        // Always read built in functions
        if (_functions.ContainsKey(token.Content.ToLower())) {
            return ReadFunctionCall(token, false);
        }
        // Only read custom functions if we have a ( following
        if (_reader.Peek().Type == TokenType.ParenthesisOpen) {
            return ReadFunctionCall(token, true);
        }

        return ReadIdentifier(token);
    }

    private Expression ReadIdentifier(Token token) {
        return new Identifier(token.Content, token.Position);
    }

    private Expression ReadFunctionCall(Token identifierToken, bool mightBeMultiplication) {
        bool expectsVector = false;
        if (_functions.TryGetValue(identifierToken.Content, out var functionData)) {
            expectsVector = (functionData.ExpectedType & FunctionExpectedType.Vector) > 0;
        }

        FunctionCall functionCall;

        Token next = _reader.Peek();
        Index start = identifierToken.Position.Start;
        Index end = identifierToken.Position.End;
        if (next.Type == TokenType.ParenthesisOpen) {
            // Flag as no longer being inside an absolute expression, as we need the closed parenthesis to close the absolute
            bool wasInsideAbsolute = _insideAbsoluteExpression;
            _insideAbsoluteExpression = false;

            List<Expression> parameters = new();
            _reader.Skip();

            while (_reader.ReachedEnd == false) {
                Expression ex = ReadExpression();
                if (ex is ErrorNode) {
                    break;
                }

                parameters.Add(ex);
                end = ex.Position.End;

                Token peek = _reader.Peek();
                if (peek.Type is TokenType.ParenthesisClose or TokenType.EndOfFile) {
                    _reader.Skip();
                    end = peek.Position.End;

                    // Go back to previous state
                    _insideAbsoluteExpression = wasInsideAbsolute;
                    break;
                }

                if (peek.Type == TokenType.Comma) {
                    _reader.Skip();
                    end = peek.Position.End;
                    continue;
                }

            }

            if (expectsVector && functionData?.ParameterCount.End.Value == 1 && parameters.Count > 1) {
                functionCall = new FunctionCall(identifierToken.Content, new[] { new VectorDeclaration(parameters, next.Position.Start..end) }, start..end);
            } else {
                functionCall = new FunctionCall(identifierToken.Content, parameters, start..end);
            }
        } else {
            Expression expression = ReadOnlyImplicitMultiplication();
            end = expression.Position.End;
            functionCall = new FunctionCall(identifierToken.Content, new[] { expression }, start..end);
        }

        if (mightBeMultiplication) {
            return new FunctionCallOrMultiplication(
                functionCall, 
                new BinaryOperation(
                    new Identifier(identifierToken.Content, identifierToken.Position),
                    BinaryOperation.OperationType.Multiply,
                    functionCall.Arguments.Length == 1 ? 
                        functionCall.Arguments[0] : 
                        new VectorDeclaration(functionCall.Arguments, start..end), 
                start..end)
            );
        }
        return functionCall;
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
            Token? hotSwappedToken = _tokenizer.TokenizeOne(token.Content, functionLength+1);
            if (hotSwappedToken == null || hotSwappedToken.Type is TokenType.Unknown or TokenType.EndOfFile) {
                return false;
            }

            Expression @base = GetLiteralOrIdentifier(hotSwappedToken);
            Expression argument = ReadOnlyImplicitMultiplication();

            function = new FunctionCall(functionName, new[] {argument, @base}, Utils.Join(token.Position, argument.Position));
            return true;
        }

        if (char.IsDigit(token.Content[functionLength])) {
            if (double.TryParse(token.Content[functionLength..], out double @base)) {
                Range newRange = (token.Position.Start.Value + functionLength)..token.Position.End;
                Expression baseLiteral = new NumberLiteral(@base.ToString(CultureInfo.InvariantCulture), newRange);
                Expression argument = ReadLiteral();

                function = new FunctionCall(functionName, new[] { argument, baseLiteral }, Utils.Join(token.Position, argument.Position));
                return true;
            }
        }

        return false;
    }
}
