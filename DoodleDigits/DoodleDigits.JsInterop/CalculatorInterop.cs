using DoodleDigits.Core;
using DoodleDigits.Core.Execution.Results;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace DoodleDigitsJsInterop;

public partial class CalculatorInterop
{
    [JSExport]
    [return:JSMarshalAs<JSType.String>]
    internal static string Calculate(string input)
    {
        var calculator = new Calculator();
        var result = calculator.Calculate(input);

        return ResultJsonConverter.CalculationToJson(result).ToJsonString();
    }
}
