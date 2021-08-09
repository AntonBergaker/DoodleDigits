﻿using System;
using System.Globalization;
using DoodleDigits.Core.Execution.Results;

namespace DoodleDigits.Core.Execution.ValueTypes {
    public class BooleanValue : Value, IConvertibleToReal, IConvertibleToBool {
        public readonly bool Value;

        public BooleanValue(bool value) : this(value, false) { }

        public BooleanValue(bool value, bool triviallyAchieved) : base(triviallyAchieved) {
            Value = value;
        }

        public override string ToString() {
            return Value ? "true" : "false";
        }

        public RealValue ConvertToReal() {
            return new RealValue(Value ? 1 : 0);
        }

        public RealValue ConvertToReal(ExecutionContext context) {
            RealValue newValue = ConvertToReal();
            context.AddResult(new ResultConversion(this, newValue, ResultConversion.ConversionType.TypeChange, context.Position));
            return newValue;
        }

        public BooleanValue ConvertToBool() {
            return this;
        }

        public BooleanValue ConvertToBool(ExecutionContext context) {
            return this;
        }

        public override bool Equals(Value? other) {
            if (other is not BooleanValue bOther) {
                return false;
            }

            return bOther.Value == Value;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public override Value Clone(bool? triviallyAchieved = null) {
            return new BooleanValue(Value, triviallyAchieved ?? this.TriviallyAchieved);
        }
    }
}