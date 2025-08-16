using DoodleDigits.Core;

var calculator = new Calculator();


while (true) {
    var message = Console.ReadLine();
    if (message == null || message == "quit") {
        break;
    }

    var calculation = calculator.Calculate(message);
    foreach (var result in calculation.Results) {
        Console.WriteLine($"> {result}");
    }
}