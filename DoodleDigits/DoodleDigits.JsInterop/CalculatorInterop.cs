using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using DoodleDigits.Core;

namespace DoodleDigits.JsInterop;

public partial class CalculatorInterop
{
    [JSExport]
    [return:JSMarshalAs<JSType.String>]
    internal static string Calculate(string input, string settingsJson)
    {
        var calculator = new Calculator();

        var settings = JsonSerializer.Deserialize(settingsJson, JsonContext.Default.CalculatorSettingsInterop);
        if (settings != null) {
            calculator.Settings = settings.ToSettings();
        }

        var result = calculator.Calculate(input);

        return ResultJsonConverter.CalculationToJson(result).ToJsonString();
    }
}

[JsonSerializable(typeof(CalculatorSettingsInterop))]
public partial class JsonContext : JsonSerializerContext { }
