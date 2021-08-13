using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Functions;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core {
    public class FunctionData {
        public readonly string[] Names;

        public readonly Func<Value[], ExecutionContext<Function>, Value> Function;

        public readonly Range ParameterCount;

        public FunctionData(string[] names, Range parameterCount, Func<Value[], ExecutionContext<Function>, Value> function) {
            Names = names;
            this.ParameterCount = parameterCount;
            Function = function;
        }

        public FunctionData(string name, Range parameterCount, Func<Value[], ExecutionContext<Function>, Value> function) : 
            this(new[] {name}, parameterCount, function) { }

        public FunctionData(string[] names, Func<Value, ExecutionContext<Function>, Value> function) : this(
            names, 1..1, (parameters, context) => function(parameters[0], context)
        ) { }

        public FunctionData(string name, Func<Value, ExecutionContext<Function>, Value> function) : this(
                new[] {name}, function
            ) { }

        public FunctionData(string[] names, Func<Value, Value, ExecutionContext<Function>, Value> function) : this(
                names, 2..2, (parameters, context) => function(parameters[0], parameters[1], context)
            ) { }

        public FunctionData(string name, Func<Value, Value, ExecutionContext<Function>, Value> function) : this(
            new[] { name }, function
        ) { }


    }

    public static class FunctionLibrary {

        public static FunctionData[] Functions = new FunctionData[] {
            new("log", 1..2, NamedFunctions.Log),
            new("root", NamedFunctions.Root),
            new("ln", NamedFunctions.Ln),
            new("sin", NamedFunctions.Sine),
            new("cos", NamedFunctions.Cosine),
            new("tan", NamedFunctions.Tangent),
            new("sec", NamedFunctions.Secant),
            new(new [] {"csc", "cosec"}, NamedFunctions.Cosecant),
            new(new [] {"cot", "cotan", "ctg"}, NamedFunctions.Cotangent),
            new(new [] {"arcsin", "asin"}, NamedFunctions.ArcSine),
            new(new [] {"arccos", "acos"}, NamedFunctions.ArcCosine),
            new(new [] {"arctan", "atan"}, NamedFunctions.ArcTangent),
            new(new [] {"arcsec", "asec"}, NamedFunctions.ArcSecant),
            new(new [] {"arccsc", "arccosec", "acsc", "acosec"}, NamedFunctions.ArcCosecant),
            new(new [] {"arccot", "arccotan", "arcctg", "acot", "acotan", "actg"}, NamedFunctions.ArcCotangent),
            new("sinh", NamedFunctions.SineHyperbolic),
            new("cosh", NamedFunctions.CosineHyperbolic),
            new("tanh", NamedFunctions.TangentHyperbolic),
            new("sech", NamedFunctions.SecantHyperbolic),
            new(new [] {"csch", "cosech"}, NamedFunctions.CosecantHyperbolic),
            new(new [] {"coth", "cotanh", "ctgh"}, NamedFunctions.CotangentHyperbolic),
            new(new [] {"arcsinh", "asinh"}, NamedFunctions.ArcSineHyperbolic),
            new(new [] {"arccosh", "acosh"}, NamedFunctions.ArcCosineHyperbolic),
            new(new [] {"arctanh", "atanh"}, NamedFunctions.ArcTangentHyperbolic),
            new(new [] {"arcsech", "asech"}, NamedFunctions.ArcSecantHyperbolic),
            new(new [] {"arccsch", "arccosech", "acsch", "acosech"}, NamedFunctions.ArcCosecantHyperbolic),
            new(new [] {"arccoth", "arccotanh", "arcctgh", "acoth", "acotanh", "actgh"}, NamedFunctions.ArcCotangentHyperbolic),
            new(new [] {"sqrt", "square_root"}, NamedFunctions.Sqrt),
            new("max", 1..^1, NamedFunctions.Max),
            new("min", 1..^1, NamedFunctions.Min),
            new("floor", NamedFunctions.Floor),
            new(new [] {"ceil", "ceiling"}, NamedFunctions.Ceil),
            new("round", NamedFunctions.Round),
            new(new [] {"gcd", "gcf"}, NamedFunctions.GreatestCommonDivisor),
            new("sign", NamedFunctions.Sign),
            new(new [] {"abs", "absolute"}, NamedFunctions.Abs),
        };

    }
}
