using DoodleDigits.Core;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DoodleDigitsJsInterop;

public static class ResultJsonConverter {
    private static KeyValuePair<string, JsonNode?> RangeToJson(Range range) {
        return new KeyValuePair<string, JsonNode?>("range",
            JsonValue.Create($"{range.Start.Value}-{range.End.Value}")
        );
    }

    public static JsonObject CalculationToJson(CalculationResult calculationResult) {

        var array = new JsonArray();
        
        foreach (var result in calculationResult.Results) {
            array.Add((JsonNode)ResultToJson(result));
        }

        return new JsonObject() {
            { "results", array }
        };
    }


    private static JsonObject ResultToJson(Result result) {
        return result switch {
            ResultValue rv => ResultValueToJson(rv),
            ResultError re => ResultErrorToJson(re),
            ResultConversion rc => ResultConversionToJson(rc),
            _ => throw new Exception("Missing handler for result type.")
        };
    }

    private static JsonObject ResultValueToJson(ResultValue resultValue) {
        return new JsonObject {
            { "type", "value" },
            RangeToJson(resultValue.Position),
            { "value", ValueToJson(resultValue.Value) }
        };
    }

    private static JsonNode ValueToJson(Value value) {
        var jsonObj = value switch {
            BooleanValue bv => BooleanValueToJson(bv),
            RealValue rv => RealValueToJson(rv),
            TooBigValue tbv => TooBigValueToJson(tbv),
            MatrixValue mv => MatrixToJson(mv),
            UndefinedValue uv => UndefinedToJson(uv),
            _ => new JsonObject() {
                { "type", "unknown"},
                { "value", value.ToString() },
            }
        };

        jsonObj.Add("trivially_achieved", value.TriviallyAchieved);
        return jsonObj;
    }

    private static JsonObject BooleanValueToJson(BooleanValue bv) {
        return new JsonObject {
            { "type", "boolean" },
            { "bool", bv.Value },
        };
    }

    private static JsonObject RealValueToJson(RealValue realValue) {
        string formPrefix = "";
        if (realValue.Form == RealValue.PresentedForm.Hex) {
            formPrefix = "0x";
        } else if (realValue.Form == RealValue.PresentedForm.Binary) {
            formPrefix = "0b";
        }
        return new JsonObject() {
            { "type", "real" },
            { "value", formPrefix + realValue.ToString(25, 30, "ᴇ") }
        };
    }

    private static JsonObject TooBigValueToJson(TooBigValue tooBigValue) {
        return new JsonObject() {
            { "type", "too_big" },
            { "value", tooBigValue.ValueSign switch {
                TooBigValue.Sign.Positive => "positive",
                TooBigValue.Sign.PositiveInfinity => "positive_infinity",
                TooBigValue.Sign.Negative => "negative",
                TooBigValue.Sign.NegativeInfinity => "negative_infinity",
                _ => throw new InvalidOperationException("Unknown value")
            } }
        };
    }

    private static JsonObject MatrixToJson(MatrixValue matrix) {
        
        static JsonNode MatrixDimensionToJson(MatrixValue.MatrixDimension matrix) {
            var array = new JsonArray();

            foreach (var element in matrix) {
                if (element is MatrixValue.MatrixDimension md) {
                    array.Add(MatrixDimensionToJson(md));
                } else if (element is MatrixValue.MatrixValueElement mve) {
                    array.Add(ValueToJson(mve.Value));
                }
            }

            return array;
        }

        return new JsonObject() {
            { "type", "matrix" },
            { "matrix", MatrixDimensionToJson(matrix.Dimension) }
        };
    }

    private static JsonObject UndefinedToJson(UndefinedValue undefined) {
        return new JsonObject() {
            { "type", "undefined" },
            { "undefined_type", undefined.Type switch {
                UndefinedValue.UndefinedType.Undefined => "undefined",
                UndefinedValue.UndefinedType.Unset => "unset",
                UndefinedValue.UndefinedType.Error => "error",
                _ => throw new Exception()
            }
            }
        };
    }

    private static JsonObject ResultErrorToJson(ResultError resultError) {
        return new JsonObject {
            { "type", "error" },
            RangeToJson(resultError.Position),
            { "message", resultError.Error }
        };
    }

    private static JsonObject ResultConversionToJson(ResultConversion resultConversion) {
        return new JsonObject {
            { "type", "conversion" },
            RangeToJson(resultConversion.Position),
            { "previous", ValueToJson(resultConversion.PreviousValue) },
            { "new", ValueToJson(resultConversion.NewValue) }
        };
    }
}
