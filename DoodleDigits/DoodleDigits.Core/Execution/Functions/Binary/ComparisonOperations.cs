using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using Rationals;

namespace DoodleDigits.Core.Execution.Functions.Binary {
    public static partial class BinaryOperations {
        public static Value Equals(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context) {
            {
                // Boolean
                if (lhs is BooleanValue bLhs && rhs is BooleanValue bRhs) {
                    return new BooleanValue(bLhs.Value == bRhs.Value);
                }
            }

            {
                // Too big values
                if (lhs is TooBigValue tooBigLhs && rhs is TooBigValue tooBigRhs) {
                    return new BooleanValue(tooBigLhs.ValueSign == tooBigRhs.ValueSign);
                }

                if (lhs is TooBigValue || rhs is TooBigValue) {
                    return new BooleanValue(false);
                }
            }

            {
                // Undefined values
                if (lhs is UndefinedValue || rhs is UndefinedValue) {
                    return new UndefinedValue();
                }
            }

            {
                // Real values
                if (lhs is RealValue realLhs && rhs is RealValue realRhs) {
                    return new BooleanValue(realLhs.Value.Equals(realRhs.Value));
                }
            }

            {
                // Fallback real
                if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                    RealValue realLhs = ctrLhs.ConvertToReal(context.ForNode(context.Node.Expressions[index]));
                    RealValue realRhs = ctrRhs.ConvertToReal(context.ForNode(context.Node.Expressions[index+1]));
                    return new BooleanValue(realLhs.Value.Equals(realRhs.Value));
                }
            }

            { // Fallback bool
                if (lhs is IConvertibleToBool ctbLhs && rhs is IConvertibleToBool ctbRhs) {
                    BooleanValue boolLhs = ctbLhs.ConvertToBool(context.ForNode(context.Node.Expressions[index]));
                    BooleanValue boolRhs = ctbRhs.ConvertToBool(context.ForNode(context.Node.Expressions[index+1]));
                    return new BooleanValue(boolLhs.Value == boolRhs.Value);
                }
            }

            return new UndefinedValue();
        }


        public static Value NotEquals(Value value0, Value value1, int index, ExecutionContext<Comparison> context) {
            Value equalsValue = Equals(value0, value1, index, context);
            if (equalsValue is BooleanValue @bool) {
                return new BooleanValue(!@bool.Value);
            }

            return equalsValue;
        }

        private static Value CompareBinaryReals(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context, Func<Rational, Rational, bool> comparisonFunction) {
            if (lhs is TooBigValue tbLhs && rhs is TooBigValue tbRhs) {
                return new BooleanValue(comparisonFunction(tbLhs.GetSimplifiedSize(), tbRhs.GetSimplifiedSize()));
            }

            if (lhs is TooBigValue tbvLhs) {
                return new BooleanValue( comparisonFunction(tbvLhs.GetSimplifiedSize(), 0) );
            }

            if (rhs is TooBigValue tbvRhs) {
                return new BooleanValue(comparisonFunction(0, tbvRhs.GetSimplifiedSize()));
            }

            if (lhs is IConvertibleToReal ctrLhs && rhs is IConvertibleToReal ctrRhs) {
                RealValue realLhs = ctrLhs.ConvertToReal(context.ForNode(context.Node.Expressions[index]));
                RealValue realRhs = ctrRhs.ConvertToReal(context.ForNode(context.Node.Expressions[index + 1]));
                return new BooleanValue(comparisonFunction(realLhs.Value, realRhs.Value) );
            }

            return new UndefinedValue();
        }

        public static Value LessThan(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context) {
            return CompareBinaryReals(lhs, rhs, index, context, (lhs, rhs) => lhs < rhs);
        }

        public static Value LessOrEqualTo(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context) {
            return CompareBinaryReals(lhs, rhs, index, context, (lhs, rhs) => lhs <= rhs);
        }

        public static Value GreaterThan(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context) {
            return CompareBinaryReals(lhs, rhs, index, context, (lhs, rhs) => lhs > rhs);
        }

        public static Value GreaterOrEqualTo(Value lhs, Value rhs, int index, ExecutionContext<Comparison> context) {
            return CompareBinaryReals(lhs, rhs, index, context, (lhs, rhs) => lhs >= rhs);
        }
    }
}

