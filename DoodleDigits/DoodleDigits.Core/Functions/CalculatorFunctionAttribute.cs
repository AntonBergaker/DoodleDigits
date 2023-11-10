namespace DoodleDigits.Core.Functions;

[Flags]
public enum FunctionExpectedType {
    None = 0,
    Real = 1 << 0,
    Boolean = 1 << 1,
    Vector = 1 << 2,
}


[AttributeUsage(AttributeTargets.Method)]
class CalculatorFunctionAttribute : Attribute {
    public CalculatorFunctionAttribute(FunctionExpectedType expects, string name, params string[] aliases) {

    }

    public CalculatorFunctionAttribute(FunctionExpectedType expects, int argumentCountMin, int argumentCountMax, string name, params string[] aliases) {

    }
}
