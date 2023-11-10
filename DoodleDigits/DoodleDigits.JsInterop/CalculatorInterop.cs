using System.Runtime.InteropServices.JavaScript;
using DoodleDigits.Core;

namespace DoodleDigitsJs.Interop;

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
