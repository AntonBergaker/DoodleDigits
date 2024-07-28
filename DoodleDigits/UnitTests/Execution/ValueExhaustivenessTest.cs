using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Functions;
using DoodleDigits.Core.Functions.Implementations.Binary;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;
using Rationals;

namespace UnitTests.Execution;
/// <summary>
/// These tests don't check the values, just makes sure that all types of values are valid
/// </summary>
class ValueExhaustivenessTest {


    private Value[] AllValues => new Value[] {
        new RealValue(0),
        new RealValue(1),
        new RealValue(1000000000),
        new RealValue(-1000000000),
        new RealValue(new Rational(1, 1000000000)),
        new RealValue(-1),
        new BooleanValue(false),
        new BooleanValue(true),
        new TooBigValue(TooBigValue.Sign.Negative),
        new TooBigValue(TooBigValue.Sign.Positive),
        new UndefinedValue(),
        new MatrixValue(new ( new MatrixValue.MatrixValueElement( new RealValue(1) ), new MatrixValue.MatrixValueElement(new RealValue(2)))),
        new MatrixValue(new ( new MatrixValue.MatrixValueElement( new RealValue(0) ), new MatrixValue.MatrixValueElement(new RealValue(0)))),
        new MatrixValue(new ( 
            new MatrixValue.MatrixValueElement( new RealValue(1) ), new MatrixValue.MatrixValueElement(new RealValue(2)),
            new MatrixValue.MatrixValueElement( new RealValue(3) ), new MatrixValue.MatrixValueElement(new RealValue(4))
        )),
    };

    private ExecutorContext MakeContext() => new ExecutorContext(new(), new());

    [Test]
    public void TestUnary() {
        ExecutorContext context = MakeContext();
        foreach (Value value in AllValues) {
            foreach (var op in UnaryOperation.AllFunctions) {
                op(value, context, new UnaryOperation(UnaryOperation.OperationType.Add, new ErrorNode()));
            }
        }

    }

    [Test]
    public void TestBinary() {
        ExecutorContext context = MakeContext();
        foreach (var op in BinaryOperation.AllFunctions) {
            foreach (Value lhs in AllValues) {
                foreach (Value rhs in AllValues) {
                    op(lhs, rhs,
                        context, new BinaryNodes(new ErrorNode(), new ErrorNode(), new ErrorNode()));
                }
            }
        }
    }

    [Test]
    public void TestNamedFunctions() {
        ExecutorContext context = MakeContext();
        int valueCount = AllValues.Length;
        Value[] allValues = AllValues;

        foreach (var function in FunctionLibrary.Functions) {
            int maxParameterCount = Math.Min(function.ParameterCount.End.GetOffset(int.MaxValue), 4);
            for (int parameterCount = function.ParameterCount.Start.Value; parameterCount <= maxParameterCount; parameterCount++) {
                Value[] parameters = new Value[parameterCount];
                for (int index = 0; index < Math.Pow(valueCount, parameterCount); index++) {
                    for (int i = 0; i < parameterCount; i++) {
                        parameters[i] = allValues[index / (int)Math.Pow(valueCount, i) % valueCount];
                    }

                    function.Function(parameters, context, new FunctionCall(function.Names[0], Enumerable.Repeat(new ErrorNode(), parameterCount) ));
                }
            }
        }
    }
}
