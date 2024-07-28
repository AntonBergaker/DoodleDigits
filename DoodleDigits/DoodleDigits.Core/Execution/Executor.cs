using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;
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

        if (root is NodeList list) {
            foreach (Expression expression in list.Nodes) {
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
            FunctionCall f => CalculateFunction(f, context),
            Comparison ec => CalculateComparison(ec, context),
            BaseCast bc => CalculateBaseCast(bc, context),
            VectorDeclaration vd => CalculateVector(vd, context),
            AmbiguousNode an => CalculateAmbiguousNode(an, context),
            ErrorNode error => new UndefinedValue(UndefinedValue.UndefinedType.Error),
            _ => throw new Exception("Expression not handled for " + expression.GetType()),
        };
    }

    private Value CalculateFunction(FunctionCall function, ExecutorContext context) {
        bool CheckParameterCount(Range parameterCount) {
            int minParameters = parameterCount.Start.Value;
            int maxParameters = parameterCount.End.GetOffset(int.MaxValue);


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
                return false;
            }
            return true;
        }

        // Built in function
        if (_functions.TryGetValue(function.Identifier, out var functionData)) {
            if (CheckParameterCount(functionData.ParameterCount) == false) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }
            return functionData.Function(function.Arguments.Select(x => Calculate(x, context)).ToArray(), context, function);
        } 
        
        // User function
        if (context.TryGetVariable(function.Identifier, out var foundVariable) && foundVariable is FunctionValue functionValue) {
            var count = functionValue.ArgumentNames.Length;
            if (CheckParameterCount(count..count) == false) {
                return new UndefinedValue(UndefinedValue.UndefinedType.Error);
            }

            var variables = new List<(string Identifier, Value Value)>();
            for (int i = 0; i < function.Arguments.Length; i++) {
                variables.Add((functionValue.ArgumentNames[i], Calculate(function.Arguments[i], context)));
            }

            context.PushVariableStack();
            foreach (var variable in variables) {
                context.AddVariable(variable.Identifier, variable.Value);
            }
            var result = Calculate(functionValue.Implementation, context);
            if (result.TriviallyAchieved) {
                result = result.Clone(false);
            }
            context.PopVariableStack();
            return result;
        } 


        _results.Add(new ResultError($"Unknown function: {function.Identifier}", function.Position));
        return new UndefinedValue(UndefinedValue.UndefinedType.Error);
    }


    private Value CalculateIdentifier(Identifier identifier, ExecutorContext context) {
        if (context.TryGetVariable(identifier.Value, out Value? variable)) {
            return variable;
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
                if (context.TryGetVariable(identifier.Value, out _) == false) {
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

                context.AddVariable(value.Value, calculatedResult.Clone(false));
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

    private Value CalculateFunctionDeclaration(FunctionDeclaration function, ExecutorContext context) {
        var functionValue = new FunctionValue(function.Identifier, function.ArgumentNames, function.Implementation, false);
        context.AddVariable(function.Identifier, functionValue);
        return functionValue;
    }

    private Value CalculateAmbiguousNode(AmbiguousNode node, ExecutorContext context) {
        return node switch {
            FunctionCallOrMultiplication fcom => CalculateFunctionCallOrMultiplication(fcom, context),
            FunctionDeclarationOrEquals fdoe => CalculateFunctionDeclarationOrEquals(fdoe, context),
            _ => new UndefinedValue(UndefinedValue.UndefinedType.Error)
        };
    }

    private Value CalculateFunctionDeclarationOrEquals(FunctionDeclarationOrEquals node, ExecutorContext context) {
        // Not used, free to become a function I guess
        if (context.TryGetVariable(node.FunctionDeclaration.Identifier, out _) == false) {
            return CalculateFunctionDeclaration(node.FunctionDeclaration, context);
        }
        return CalculateComparison(node.EqualsComparison, context);
    }

    private Value CalculateFunctionCallOrMultiplication(FunctionCallOrMultiplication node, ExecutorContext context) {
        if (context.TryGetVariable(node.Function.Identifier, out var value)) {
            if (value is FunctionValue function) {
                return CalculateFunction(node.Function, context);
            }
        }
        return CalculateBinary(node.Multiplication, context);
    }
}
