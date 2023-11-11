# Doodle Digits

<img src="https://raw.githubusercontent.com/AntonBergaker/DoodleDigits/main/Marketing%20Images/App%20Preview.png" width="600">

Doodle Digits is an inline calculator where you write equations into a large textbox and the answers are given to you as you type.  
This approach lets you easily go back and change things in your equation and is easy and intuitive to use.

## Features
* Variables that can hold values
* Arbitrary precision numbers allow you to calculate very large numbers, some high limits are put in for performance reasons
* Autosaving that quickly lets you go back to where you were
* A large library of math functions
* Bitwise operations (keywords are `bxor`, `bor` and `band` to not clash with some other math notations)
* Hex and binary numbers (`0xE32C2C`, `0b10`)
* Vectors and matrices (`x = (1, 2)`, `det [[3, 4], [5, 6]]`) 

## Download and Install
Downloads and installation instructions can be found on the [releases](https://github.com/AntonBergaker/DoodleDigits/releases) page.

## Doodle Digits as a library
You can use the Doodle Digits math core as a standalone math library for C#. The nuget package can be found [here](https://www.nuget.org/packages/DoodleDigits.Core).
```csharp
using DoodleDigits.Core;

// Create a new calculator. The default constructor will use the functions inside FunctionLibrary.Functions and ConstantLibary.Constants.
// You can implement custom functions and constants using the Calculator(IEnumerable<FunctionData> functions, IEnumerable<Constant> constants) constructor.
var calculator = new Calculator();

// Calculate the input
var result = calculator.Calculate("5 + 5");

// Find the result with a value. A calculation can return multiple results, including error messages and hints. The ResultValue is the result of a calculation.
var resultValue = result.Results.OfType<ResultValue>().LastOrDefault();
```

## Contributing
As in most projects, make sure whatever you submit is unit tested if necessary and that all tests pass.  
Commits with emoji are preferred.
I try to stay on the latest version of C# and .NET because it's nice.