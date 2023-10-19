using System;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.ValueTypes;
using DoodleDigits.Core.Parsing.Ast;
using Rationals;

namespace DoodleDigits.Core.Functions.Implementations.Binary {
    public static partial class BinaryOperations {
        public static Value Equals(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
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
                    return new UndefinedValue((lhs as UndefinedValue)?.Type ?? (rhs as UndefinedValue)!.Type);
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
                    RealValue realLhs = ctrLhs.ConvertToReal(context, nodes.Lhs);
                    RealValue realRhs = ctrRhs.ConvertToReal(context, nodes.Rhs);
                    return new BooleanValue(realLhs.Value.Equals(realRhs.Value));
                }
            }

            { // Fallback bool
                if (lhs is IConvertibleToBool ctbLhs && rhs is IConvertibleToBool ctbRhs) {
                    BooleanValue boolLhs = ctbLhs.ConvertToBool(context, nodes.Lhs);
                    BooleanValue boolRhs = ctbRhs.ConvertToBool(context, nodes.Rhs);
                    return new BooleanValue(boolLhs.Value == boolRhs.Value);
                }
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }


        public static Value NotEquals(Value value0, Value value1, ExecutionContext context, BinaryNodes nodes) {
            Value equalsValue = Equals(value0, value1, context, nodes);
            if (equalsValue is BooleanValue @bool) {
                return new BooleanValue(!@bool.Value);
            }

            return equalsValue;
        }

        private static Value CompareBinaryReals(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes, Func<Rational, Rational, bool> comparisonFunction) {
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
                RealValue realLhs = ctrLhs.ConvertToReal(context, nodes.Lhs);
                RealValue realRhs = ctrRhs.ConvertToReal(context, nodes.Rhs);
                return new BooleanValue(comparisonFunction(realLhs.Value, realRhs.Value) );
            }

            return new UndefinedValue(UndefinedValue.UndefinedType.Error);
        }

        public static Value LessThan(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return CompareBinaryReals(lhs, rhs, context, nodes, (lhs, rhs) => lhs < rhs);
        }

        public static Value LessOrEqualTo(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return CompareBinaryReals(lhs, rhs, context, nodes, (lhs, rhs) => lhs <= rhs);
        }

        public static Value GreaterThan(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return CompareBinaryReals(lhs, rhs, context, nodes, (lhs, rhs) => lhs > rhs);
        }

        public static Value GreaterOrEqualTo(Value lhs, Value rhs, ExecutionContext context, BinaryNodes nodes) {
            return CompareBinaryReals(lhs, rhs, context, nodes, (lhs, rhs) => lhs >= rhs);
        }
    }
}

