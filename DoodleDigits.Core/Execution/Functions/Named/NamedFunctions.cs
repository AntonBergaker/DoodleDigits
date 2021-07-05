using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution {
    public static class NamedFunctions {
        /*
        new("log", 1..2, (a) => 
        a.Length == 1 ? Math.Log(a[0]) : Math.Log(a[0], a[1])),
        new("ln", a => Math.Log(a)),
        new("sin", a => Math.Sin(a)),
        new("cos", a => Math.Cos(a)),
        new("tan", a => Math.Tan(a)),
        new("sqrt", a => Math.Sqrt(a)),
        new("square_root", a => Math.Sqrt(a)),
        new("max", 1..^1, (a) => a.Max()),
        new("min", 1..^1, (a) => a.Min()),
        */

        private static RealValue ConvertArgumentToReal(Value value, int index, ExecutionContext<Function> context) {
            Function func = context.Node;

            if (value is RealValue real) {
                return real;
            }

            if (value is BooleanValue @bool) {
                RealValue newValue = @bool.ConvertToReal();
                context.AddResult(new ResultConversion(@bool, newValue, func.Arguments[index].Position));
            }

            throw new NotImplementedException();
        }
        


        public static Value Log(Value[] values, ExecutionContext<Function> context) {
            if (values.Length == 1) {
                return new RealValue(Math.Log10( ConvertArgumentToReal(values[0], 0, context).Value ));
            }

            return new RealValue(Math.Log(
                ConvertArgumentToReal(values[0], 0, context).Value,
                ConvertArgumentToReal(values[1], 1, context).Value
            ));
        }

        public static Value Root(Value value, Value root, ExecutionContext<Function> context) {
            double dValue = ConvertArgumentToReal(value, 0, context).Value;
            double dRoot = ConvertArgumentToReal(root, 1, context).Value;
            return new RealValue(Math.Pow(dValue, 1/dRoot));
        }

        public static Value Ln(Value value, ExecutionContext<Function> context) {
            return new RealValue(Math.Log(ConvertArgumentToReal(value, 0, context).Value));
        }

        public static Value Sin(Value value, ExecutionContext<Function> context) {
            return new RealValue(Math.Sin(ConvertArgumentToReal(value, 0, context).Value));
        }

        public static Value Cos(Value value, ExecutionContext<Function> context) {
            return new RealValue(Math.Sin(ConvertArgumentToReal(value, 0, context).Value));
        }

        public static Value Tan(Value value, ExecutionContext<Function> context) {
            return new RealValue(Math.Sin(ConvertArgumentToReal(value, 0, context).Value));
        }

        public static Value Sqrt(Value value, ExecutionContext<Function> context) {
            return new RealValue(Math.Sqrt(ConvertArgumentToReal(value, 0, context).Value));
        }

        public static Value Max(Value[] values, ExecutionContext<Function> context) {
            return new RealValue(values.Select((x, i) => ConvertArgumentToReal(x, i, context).Value).Max()) ;
        }

        public static Value Min(Value[] values, ExecutionContext<Function> context) {
            return new RealValue(values.Select((x, i) => ConvertArgumentToReal(x, i, context).Value).Min());
        }
    }
}
