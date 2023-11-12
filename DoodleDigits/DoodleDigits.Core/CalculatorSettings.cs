namespace DoodleDigits.Core;
public class CalculatorSettings {
    public AngleUnits AngleUnit { get; set; }
    
    public CalculatorSettings() {
        AngleUnit = AngleUnits.Radians;
    }
}
