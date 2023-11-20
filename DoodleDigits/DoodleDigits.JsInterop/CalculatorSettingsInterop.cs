using DoodleDigits.Core;
using System.Text.Json.Serialization;

namespace DoodleDigits.JsInterop;

public class CalculatorSettingsInterop {
    [JsonConverter(typeof(JsonStringEnumConverter<AngleUnits>))]
    [JsonPropertyName("angleUnit")]
    public AngleUnits AngleUnit { get; }

    
    
    [JsonConstructor]
    public CalculatorSettingsInterop(AngleUnits angleUnit) { 
        AngleUnit = angleUnit;
    }

    public CalculatorSettings ToSettings() {
        return new CalculatorSettings() {
            AngleUnit = AngleUnit
        };
    }
}
