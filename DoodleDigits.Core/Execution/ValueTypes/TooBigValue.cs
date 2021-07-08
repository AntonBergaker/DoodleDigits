using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core.Ast;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits.Core.Execution {
    public class TooBigValue : Value, IConvertibleToBool {
        public enum Sign {
            Positive,
            PositiveInfinity,
            Negative,
            NegativeInfinity,
        }

        public readonly Sign ValueSign;

        public TooBigValue(Sign sign) {
            ValueSign = sign;
        }

        public override string ToString() {
            return "Very big";
        }

        public override bool IsAbstract => true;

        public bool IsPositive => ValueSign is Sign.Positive or Sign.PositiveInfinity;

        public Value Negate() {
            return new TooBigValue(ValueSign switch {
                Sign.Positive => Sign.Negative,
                Sign.PositiveInfinity => Sign.NegativeInfinity,
                Sign.Negative => Sign.Positive,
                Sign.NegativeInfinity => Sign.PositiveInfinity,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        public BooleanValue ConvertToBool() {
            return new BooleanValue(IsPositive);
        }

        public BooleanValue ConvertToBool(ExecutionContext context, Range position) {
            BooleanValue newValue = ConvertToBool();
            context.AddResult(new ResultConversion(this, newValue, position));
            return newValue;
        }

        public int GetSimplifiedSize() {
            return ValueSign switch {
                Sign.PositiveInfinity => 2,
                Sign.Positive => 1,
                Sign.Negative => -1,
                Sign.NegativeInfinity => -2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
