using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using DoodleDigits.Core;

namespace DoodleDigitsJs.Interop;

public partial class CalculatorInterop
{
    [JSExport]
    [return:JSMarshalAs<JSType.String>]
    internal static string Calculate(string input, string settingsJson)
    {
        var calculator = new Calculator();

        var settings = JsonSerializer.Deserialize(settingsJson, JsonContext.Default.CalculatorSettings);
        if (settings != null) {
            calculator.Settings = settings;
        }

        var result = calculator.Calculate(input);

        return ResultJsonConverter.CalculationToJson(result).ToJsonString();
    }
}

[JsonSerializable(typeof(CalculatorSettings))]
public partial class JsonContext : JsonSerializerContext { }
