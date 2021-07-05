using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Functions;
using DoodleDigits.Core.Execution.Functions.Binary;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution {

    public class ExecutionResult {
        public readonly Result[] Results;
        public ExecutionResult(Result[] results) {
            Results = results;
        }
    }

    public class Executor {
        private ExecutionContext context;
        private readonly List<Result> results;
        private readonly AstBuilder builder;
        private readonly Dictionary<string, FunctionData> functions;

        public Executor(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) {
            results = new List<Result>();
            builder = new AstBuilder(functions.Select(x => x.Name));
            this.functions = functions.ToDictionary(x => x.Name);
            context = new ExecutionContext(constants);
        }

        public ExecutionResult Calculate(string input) {
            results.Clear();

            var result = builder.Build(input);

            //errors.AddRange(result.Errors);

            if (result.Root is ExpressionList list) {
                foreach (Expression expression in list.Expressions) {
                    results.Add(new ResultValue(Calculate(expression), expression.Position));
                }
            } else if (result.Root is Expression ex) {
                results.Add(new ResultValue(Calculate(ex), ex.Position));
            }

            results.Sort((a, b) => a.Position.Start.GetOffset(input.Length) - b.Position.Start.GetOffset(input.Length));

            return new ExecutionResult( results.ToArray());
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
                case ErrorNode error:
                    return new RealValue(0);
                default: throw new Exception("Expression not handled for " + expression.GetType());
            }

        }

        private Value Calculate(Function function) {
            if (functions.TryGetValue(function.Identifier, out var functionData)) {

                int minParameters = functionData.ParameterCount.Start.Value;
                int maxParameters = functionData.ParameterCount.End.GetOffset(int.MaxValue);

;                if (function.Arguments.Length < minParameters ||
                    function.Arguments.Length > maxParameters) {

                    results.Add(new ResultError(minParameters == maxParameters ? 
                        $"Function expects {minParameters} parameters" : 
                        $"Function expects between {minParameters} and {maxParameters} parameters", 
                        function.Position));
                    return new RealValue(0);
                }

                return functionData.Function(function.Arguments.Select(x => Calculate(x)).ToArray(), context.ForNode(function));
            }

            results.Add(new ResultError($"Unknown function: {function.Identifier}", function.Position));
            return new RealValue(0);
        }

        private Value Calculate(Identifier identifier) {
            if (context.Constants.TryGetValue(identifier.Value, out Value? constantValue)) {
                return constantValue;
            }

            if (context.Variables.TryGetValue(identifier.Value, out Value? variableValue)) {
                return variableValue;
            }

            results.Add(new ResultError("Unknown identifier", identifier.Position));
            return new RealValue(0);
        }

        private Value Calculate(NumberLiteral numberLiteral) {

            if (double.TryParse(numberLiteral.Number.Replace("_", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out double result)) {
                return new RealValue(result);
            }

            return new RealValue(0);
        }

        private Value Calculate(UnaryOperation unaryOperation) {
            Value value = Calculate(unaryOperation.Value);
            return unaryOperation.Operation switch {
                UnaryOperation.OperationType.Add => UnaryOperations.UnaryPlus(value, context.ForNode(unaryOperation)),
                UnaryOperation.OperationType.Subtract => UnaryOperations.UnaryNegate(value, context.ForNode(unaryOperation)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        private Value Calculate(BinaryOperation bo) {
            if (bo.Operation == BinaryOperation.OperationType.Equals) {
                if (bo.Left is Identifier id0) {
                    return BinaryOperations.Equals(id0, Calculate(bo.Right), context.ForNode(bo));
                }
                if (bo.Right is Identifier id1) {
                    return BinaryOperations.Equals(id1, Calculate(bo.Left), context.ForNode(bo));
                }
            }

            Value lhs = Calculate(bo.Left);
            Value rhs = Calculate(bo.Right);

            Func<Value, Value, ExecutionContext<BinaryOperation>, Value> func = bo.Operation switch {
                BinaryOperation.OperationType.Add => BinaryOperations.Add,
                BinaryOperation.OperationType.Subtract => BinaryOperations.Subtract,
                BinaryOperation.OperationType.Divide => BinaryOperations.Divide,
                BinaryOperation.OperationType.Multiply => BinaryOperations.Multiply,
                BinaryOperation.OperationType.Modulus => BinaryOperations.Modulus,
                BinaryOperation.OperationType.Power => BinaryOperations.Power,
                BinaryOperation.OperationType.Equals => BinaryOperations.Equals,
                BinaryOperation.OperationType.NotEquals => BinaryOperations.NotEquals,
                BinaryOperation.OperationType.LessThan => BinaryOperations.LessThan,
                BinaryOperation.OperationType.LessOrEqualTo => BinaryOperations.LessOrEqualTo,
                BinaryOperation.OperationType.GreaterThan => BinaryOperations.GreaterThan,
                BinaryOperation.OperationType.GreaterOrEqualTo => BinaryOperations.GreaterOrEqualTo,
                _ => throw new ArgumentOutOfRangeException(),
            };

            return func(lhs, rhs, context.ForNode(bo));
        }

    }
}
