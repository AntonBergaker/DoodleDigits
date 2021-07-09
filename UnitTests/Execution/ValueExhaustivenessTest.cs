using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using NUnit.Framework;

namespace UnitTests.Execution {
    /// <summary>
    /// These tests don't check the values, just makes sure that all types of values are valid
    /// </summary>
    class ValueExhaustivenessTest {


        private Value[] AllValues => new Value[] { 
            new RealValue(0),
            new RealValue(1),
            new RealValue(1000000000),
            new RealValue(-1000000000),
            new RealValue(-1),
            new BooleanValue(false), 
            new BooleanValue(true),
            new TooBigValue(TooBigValue.Sign.Negative), 
            new TooBigValue(TooBigValue.Sign.Positive),
            new UndefinedValue(),
        };

        private ExecutionContext MakeContext() => new ExecutionContext(new Constant[0]);

        [Test]
        public void TestUnary() {
            ExecutionContext context = MakeContext();
            foreach (Value value in AllValues) {
                foreach (var op in UnaryOperation.AllFunctions) {
                    op(value, context.ForNode(new UnaryOperation(UnaryOperation.OperationType.Add, new ErrorNode())));
                }
            }

        }

        [Test]
        public void TestBinary() {
            ExecutionContext context = MakeContext();
            foreach (var op in BinaryOperation.AllFunctions) {
                foreach (Value lhs in AllValues) {
                    foreach (Value rhs in AllValues) {
                        op(lhs, rhs,
                            context.ForNode(new BinaryOperation(new ErrorNode(), BinaryOperation.OperationType.Add, new ErrorNode())));
                    }
                }
            }
        }

        [Test]
        public void TestNamedFunctions() {
            ExecutionContext context = MakeContext();
            int valueCount = AllValues.Length;
            Value[] allValues = AllValues;

            foreach (var function in FunctionLibrary.Functions) {
                int maxParameterCount = Math.Min(function.ParameterCount.End.GetOffset(int.MaxValue), 5);
                for (int parameterCount = function.ParameterCount.Start.Value; parameterCount <= maxParameterCount; parameterCount++) {
                    Value[] parameters = new Value[parameterCount];
                    for (int index = 0; index < Math.Pow(valueCount, parameterCount); index++) {
                        for (int i = 0; i < parameterCount; i++) {
                            parameters[i] = allValues[index / (int)Math.Pow(valueCount, i) % valueCount];
                        }

                        function.Function(parameters, context.ForNode(new Function(function.Names[0], Enumerable.Repeat(new ErrorNode(), parameterCount) )));
                    }
                }
            }
        }
    }
}
