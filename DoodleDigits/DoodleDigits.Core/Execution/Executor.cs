using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution;

public class ExecutionResult {
    public readonly Result[] Results;
    public ExecutionResult(Result[] results) {
        Results = results;
    }
}

public class Executor {
    private readonly List<Result> _results;
    private readonly Dictionary<string, FunctionData> _functions;
    private readonly Dictionary<string, Value> _constants;

    public Executor(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) {
        _results = new List<Result>();
        _functions = new Dictionary<string, FunctionData>();
        foreach (FunctionData functionData in functions) {
            foreach (string name in functionData.Names) {
                _functions.Add(name, functionData);
            }
        }
        _constants = constants.ToDictionary(x => x.Name, x => x.Value);
    }

    public ExecutionResult Execute(AstNode root, CalculatorSettings settings) {
        _results.Clear();
        var context = new ExecutorContext(settings, _constants);

        if (root is ExpressionList list) {
            foreach (Expression expression in list.Expressions) {
                _results.Add(new ResultValue(Calculate(expression, context), expression.Position));
            }
        } else if (root is Expression ex) {
            _results.Add(new ResultValue(Calculate(ex, context), ex.Position));
        }

        _results.AddRange(context.Results);

        _results.Sort((a, b) => a.Position.Start.Value - b.Position.Start.Value);

        return new ExecutionResult(_results.ToArray());
    }


    private Value Calculate(Expression expression, ExecutorContext context) {

        return expression switch {
            BinaryOperation bo => CalculateBinary(bo, context),
            UnaryOperation uo => CalculateUnary(uo, context),
            NumberLiteral nl => CalculateNumber(nl, context),
            Identifier id => CalculateIdentifier(id, context),
            Function f => CalculateFunction(f, context),
            Comparison ec => CalculateComparison(ec, context),
            BaseCast bc => CalculateBaseCast(bc, context),
            VectorDeclaration vd => CalculateVector(vd, context),
            ErrorNode error => new UndefinedValue(UndefinedValue.UndefinedType.Error),
            _ => throw new Exception("Expression not handled for " + expression.GetType()),
        };
    }

    private Value CalculateFunction(Function function, ExecutorContext context) {
        if (_functions.TryGetValue(function.Identifier, out var functionData) == false) {
            _results.Add(new ResultError($"Unknown function: {function.Identifier}", function.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        int minParameters = functionData.ParameterCount.Start.Value;
        int maxParameters = functionData.ParameterCount.End.GetOffset(int.MaxValue);


        if (function.Arguments.Length < minParameters ||
            function.Arguments.Length > maxParameters) {
            string errorMessage;
            if (minParameters == maxParameters) {
                errorMessage = $"Function needs {minParameters} parameters";
            } else if (maxParameters == int.MaxValue) {
                errorMessage = $"Function needs at least {minParameters} parameters";
            } else {
                errorMessage = $"Function needs between {minParameters} and {maxParameters} parameters";
            }

            _results.Add(new ResultError(errorMessage, function.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        return functionData.Function(function.Arguments.Select(x => Calculate(x, context)).ToArray(), context, function);
    }

    private Value CalculateIdentifier(Identifier identifier, ExecutorContext context) {
        if (context.Constants.TryGetValue(identifier.Value, out Value? constantValue)) {
            return constantValue;
        }

        if (context.Variables.TryGetValue(identifier.Value, out Value? variableValue)) {
            return variableValue;
        }

        _results.Add(new ResultError("Unknown identifier", identifier.Position));
        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    private Value CalculateNumber(NumberLiteral numberLiteral, ExecutorContext context) {
        string number = numberLiteral.Number;
        int @base = 10;
        RealValue.PresentedForm form = RealValue.PresentedForm.Decimal;

        if (number.StartsWith("0x")) {
            @base = 16;
            number = number[2..];
            form = RealValue.PresentedForm.Hex;
        }

        if (number.StartsWith("0b")) {
            @base = 2;
            number = number[2..];
            form = RealValue.PresentedForm.Binary;
        }

        if (RationalUtils.TryParse(number, out Rational result, 200, @base)) {
            return new RealValue(result, true, form);
        }

        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }

    private Value CalculateUnary(UnaryOperation unaryOperation, ExecutorContext context) {
        
        Value value = Calculate(unaryOperation.Value, context);

        UnaryOperation.OperationFunction func = UnaryOperation.GetFunctionFromType(unaryOperation.Operation);
        
        return func(value, context, unaryOperation);
    }

    private Value CalculateComparison(Comparison comparison, ExecutorContext context) {

        Value? CalculateExpression(Expression expression) {
            if (expression is Identifier identifier) {
                if (context.Variables.ContainsKey(identifier.Value) == false &&
                    context.Constants.ContainsKey(identifier.Value) == false) {
                    return null;
                }
            }
            var a = Calculate(expression, context);
            return a;
        }

        Value?[] calculatedResults = comparison.Expressions.Select(x => CalculateExpression(x)).ToArray();

        bool isAssignmentChain = 
            calculatedResults.Count(x => x != null) == 1 && 
            comparison.Signs.All(x => x == Comparison.ComparisonType.Equals);

        if (isAssignmentChain) {
            Value? calculatedResult = calculatedResults.First(x => x != null);
            if (calculatedResult == null) {
                throw new Exception("This shouldn't be possible");
            }

            for (var i = 0; i < comparison.Expressions.Length; i++) {
                if (calculatedResults[i] != null) {
                    continue;
                }
                Identifier value = (Identifier)comparison.Expressions[i];

                context.Variables[value.Value] = calculatedResult.Clone(false);
            }

            return calculatedResult;
        }
        else {
            for (int i = 0; i < comparison.Signs.Length; i++) {
                var type = comparison.Signs[i];
                Value lhs = calculatedResults[i] ?? Calculate(comparison.Expressions[i], context);
                Value rhs = calculatedResults[i + 1] ?? Calculate(comparison.Expressions[i + 1], context);

                Comparison.BinaryEqualsFunction func = Comparison.GetOperationFromType(type);
                Value result = func(lhs, rhs, context, 
                    new BinaryNodes(comparison, comparison.Expressions[i], comparison.Expressions[i+1]));
                
                if (result is not BooleanValue booleanValue) {
                    return new UndefinedValue(UndefinedValue.UndefinedType.Error);
                }

                if (booleanValue.Value == false) {
                    return new BooleanValue(false);
                }
            }

            return new BooleanValue(true);
        }
    }

    private Value CalculateBinary(BinaryOperation bo, ExecutorContext context) {
        Value lhs = Calculate(bo.Lhs, context);
        Value rhs = Calculate(bo.Rhs, context);

        BinaryOperation.OperationFunction func = BinaryOperation.GetOperationFromType(bo.Operation);

        return func(lhs, rhs, context, new BinaryNodes(bo));
    }

    private Value CalculateBaseCast(BaseCast baseCast, ExecutorContext context) {
        Value expression = Calculate(baseCast.Expression, context);

        if (expression is TooBigValue) {
            return expression;
        }

        if (expression is RealValue realValue) {
            RealValue.PresentedForm form = baseCast.Target switch {
                BaseCast.TargetType.Hex => RealValue.PresentedForm.Hex,
                BaseCast.TargetType.Binary => RealValue.PresentedForm.Binary,
                BaseCast.TargetType.Decimal => RealValue.PresentedForm.Decimal,
                _ => RealValue.PresentedForm.Unset
            };

            return realValue.Clone(triviallyAchieved: false, form: form);

        }

        return expression;
    }

    private Value CalculateVector(VectorDeclaration vectorDeclaration, ExecutorContext context) {
        MatrixValue.MatrixDimension? InternalCalculate(VectorDeclaration vectorDeclaration) {
            List<MatrixValue.IMatrixElement> elements = new();

            foreach (Expression expression in vectorDeclaration.Expressions) {
                if (expression is VectorDeclaration vd) {
                    var dimension = InternalCalculate(vd);
                    if (dimension == null) {
                        return null;
                    }
                    elements.Add(dimension);
                } else {
                    Value result = Calculate(expression, context);
                    if (result is MatrixValue mv) {
                        elements.Add(mv.Dimension);
                    }
                    else {
                        elements.Add(new MatrixValue.MatrixValueElement(result));
                    }
                }
            }

            return new MatrixValue.MatrixDimension(elements);
        }

        MatrixValue.MatrixDimension? dimension = InternalCalculate(vectorDeclaration);
        if (dimension == null) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }
        MatrixValue val = new MatrixValue(dimension, true);
        if (val.IsValid == false) {
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        return val;
    }
}
