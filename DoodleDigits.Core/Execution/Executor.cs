using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Functions.Binary;

namespace DoodleDigits.Core.Execution {

    public class ExecutionResult {
        public readonly string[] Results;
        public ExecutionResult(string[] results) {
            Results = results;
        }
    }

    public class Executor {
        private ExecutionContext context;
        private readonly List<Error> errors;
        private readonly AstBuilder builder;
        private readonly Dictionary<string, FunctionData> functions;

        public Executor(IEnumerable<FunctionData> functions) {
            errors = new List<Error>();
            builder = new AstBuilder(functions.Select(x => x.Name));
            this.functions = functions.ToDictionary(x => x.Name);
            context = new ExecutionContext();
        }

        public ExecutionResult Calculate(string input) {
            errors.Clear();

            var result = builder.Build(input);

            errors.AddRange(result.Errors);

            List<string> results = new();

            if (result.Root is ExpressionList list) {
                foreach (Expression expression in list.Expressions) {
                    results.Add(Calculate(expression).ToString());
                }
            } else if (result.Root is Expression ex) {
                results.Add(Calculate(ex).ToString());
            }

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

                    errors.Add(new Error(function.Position, minParameters == maxParameters ? 
                        $"Function expects {minParameters} parameters" : 
                        $"Function expects between {minParameters} and {maxParameters} parameters"));
                    return new RealValue(0);
                }

                context.Node = function;
                return functionData.Function(function.Arguments.Select(x => Calculate(x)).ToArray(), context);
            }

            errors.Add(new Error(function.Position, $"Unknown function: {function.Identifier}"));
            return new RealValue(0);
        }

        private Value Calculate(Identifier identifier) {
            return new RealValue(0);
        }

        private Value Calculate(NumberLiteral numberLiteral) {

            if (double.TryParse(numberLiteral.Number.Replace("_", ""), out double result)) {
                return new RealValue(result);
            }

            return new RealValue(0);
        }

        private Value Calculate(UnaryOperation unaryOperation) {
            Value value = Calculate(unaryOperation.Value);
            return unaryOperation.Operation switch {
                UnaryOperation.OperationType.Add => value,
                UnaryOperation.OperationType.Subtract => new RealValue( - (value as RealValue).Value ),
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        private Value Calculate(BinaryOperation bo) {
            context.Node = bo;
            Value lhs = Calculate(bo.Left);
            Value rhs = Calculate(bo.Right);

            Func<Value, Value, ExecutionContext, Value> func = bo.Operation switch {
                BinaryOperation.OperationType.Add => RealBinaryOperations.Add,
                BinaryOperation.OperationType.Subtract => RealBinaryOperations.Subtract,
                BinaryOperation.OperationType.Divide => RealBinaryOperations.Divide,
                BinaryOperation.OperationType.Multiply => RealBinaryOperations.Multiply,
                BinaryOperation.OperationType.Modulus => RealBinaryOperations.Modulus,
                BinaryOperation.OperationType.Power => RealBinaryOperations.Power,
                _ => throw new ArgumentOutOfRangeException(),
            };

            return func(lhs, rhs, context);
        }

    }
}
