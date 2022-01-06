using System;
using System.Collections.Generic;
using System.Linq;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using DoodleDigits.Core.Utilities;
using Rationals;

namespace DoodleDigits.Core.Execution {

    public class ExecutionResult {
        public readonly Result[] Results;
        public ExecutionResult(Result[] results) {
            Results = results;
        }
    }

    public class Executor {
        private readonly ExecutionContext context;
        private readonly List<Result> results;
        private readonly Dictionary<string, FunctionData> functions;

        public Executor(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) {
            results = new List<Result>();
            this.functions = new Dictionary<string, FunctionData>();
            foreach (FunctionData functionData in functions) {
                foreach (string name in functionData.Names) {
                    this.functions.Add(name, functionData);
                }
            }
            context = new ExecutionContext(constants);
        }

        public ExecutionResult Execute(AstNode root) {
            results.Clear();
            context.Clear();

            if (root is ExpressionList list) {
                foreach (Expression expression in list.Expressions) {
                    results.Add(new ResultValue(Calculate(expression), expression.Position));
                }
            } else if (root is Expression ex) {
                results.Add(new ResultValue(Calculate(ex), ex.Position));
            }

            results.AddRange(context.Results);

            results.Sort((a, b) => a.Position.Start.Value - b.Position.Start.Value);

            return new ExecutionResult(results.ToArray());
        }


        private Value Calculate(Expression expression) {

            switch (expression) {
                case BinaryOperation bo:
                    return Calculate(bo);
                case UnaryOperation uo:
                    return Calculate(uo);
                case NumberLiteral nl:
                    return Calculate(nl);
                case Identifier id:
                    return Calculate(id);
                case Function f:
                    return Calculate(f);
                case Comparison ec:
                    return Calculate(ec);
                case BaseCast bc:
                    return Calculate(bc);
                case VectorDecleration vd:
                    return Calculate(vd);
                case ErrorNode error:
                    return new UndefinedValue(UndefinedValue.UndefinedType.Error);
                default: throw new Exception("Expression not handled for " + expression.GetType());
            }

        }

        private Value Calculate(Function function) {
            if (functions.TryGetValue(function.Identifier, out var functionData)) {

                int minParameters = functionData.ParameterCount.Start.Value;
                int maxParameters = functionData.ParameterCount.End.GetOffset(int.MaxValue);


;                if (function.Arguments.Length < minParameters ||
                    function.Arguments.Length > maxParameters) {

                    string errorMessage;
                    if (minParameters == maxParameters) {
                        errorMessage = $"Function needs {minParameters} parameters";
                    } else if (maxParameters == int.MaxValue) {
                        errorMessage = $"Function needs at least {minParameters} parameters";
                    }
                    else {
                        errorMessage = $"Function expects between {minParameters} and {maxParameters} parameters";
                    }

                    results.Add(new ResultError(errorMessage, function.Position));
                    return new UndefinedValue(UndefinedValue.UndefinedType.Error);
                }

                return functionData.Function(function.Arguments.Select(x => Calculate(x)).ToArray(), context.ForNode(function));
            }

            results.Add(new ResultError($"Unknown function: {function.Identifier}", function.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        private Value Calculate(Identifier identifier) {
            if (context.Constants.TryGetValue(identifier.Value, out Value? constantValue)) {
                return constantValue;
            }

            if (context.Variables.TryGetValue(identifier.Value, out Value? variableValue)) {
                return variableValue;
            }

            results.Add(new ResultError("Unknown identifier", identifier.Position));
            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        private Value Calculate(NumberLiteral numberLiteral) {
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

        private Value Calculate(UnaryOperation unaryOperation) {
            
            Value value = Calculate(unaryOperation.Value);

            UnaryOperation.OperationFunction func = UnaryOperation.GetFunctionFromType(unaryOperation.Operation);
            
            return func(value, context.ForNode(unaryOperation));
        }

        private Value Calculate(Comparison comparison) {

            Value? CalculateExpression(Expression expression) {
                if (expression is Identifier identifier) {
                    if (context.Variables.ContainsKey(identifier.Value) == false &&
                        context.Constants.ContainsKey(identifier.Value) == false) {
                        return null;
                    }
                }
                return Calculate(expression);
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
                    Value lhs = calculatedResults[i] ?? Calculate(comparison.Expressions[i]);
                    Value rhs = calculatedResults[i + 1] ?? Calculate(comparison.Expressions[i + 1]);

                    Comparison.BinaryEqualsFunction func = Comparison.GetOperationFromType(type);
                    var equalsContext = context.ForNode(comparison);
                    Value result = func(lhs, rhs, i, equalsContext);
                    
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

        private Value Calculate(BinaryOperation bo) {
            Value lhs = Calculate(bo.Lhs);
            Value rhs = Calculate(bo.Rhs);

            BinaryOperation.OperationFunction func = BinaryOperation.GetOperationFromType(bo.Operation);

            return func(lhs, rhs, context.ForNode(bo));
        }

        private Value Calculate(BaseCast baseCast) {
            Value expression = Calculate(baseCast.Expression);

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

        private Value Calculate(VectorDecleration vectorDecleration) {
            MatrixValue.MatrixDimension? InternalCalculate(VectorDecleration vectorDecleration) {
                List<MatrixValue.IMatrixElement> elements = new();

                foreach (Expression expression in vectorDecleration.Expressions) {
                    if (expression is VectorDecleration vd) {
                        var dimension = InternalCalculate(vd);
                        if (dimension == null) {
                            return null;
                        }
                        elements.Add(dimension);
                    } else {
                        Value result = Calculate(expression);
                        if (result is MatrixValue mv) {
                            elements.Add(mv.Dimension);
                        }
                        else if (result is IConvertibleToReal realConvertible) {
                            RealValue realValue = realConvertible.ConvertToReal(context.ForNode(expression));
                            elements.Add(new MatrixValue.MatrixValueElement(realValue));
                        } else {
                            return null;
                        }
                    }
                }

                return new MatrixValue.MatrixDimension(elements);
            }

            MatrixValue.MatrixDimension? dimension = InternalCalculate(vectorDecleration);
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
}
