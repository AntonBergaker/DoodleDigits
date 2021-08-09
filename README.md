# Doodle Digits

<img src="https://raw.githubusercontent.com/AntonBergaker/DoodleDigits/main/Marketing%20Images/App%20Preview.png" width="600">

Doodle Digits is an inline calculator where you write equations into a large textbox and the answers are given to you as you type.  
This approach lets you easily go back and change things in your equation and is easy and intuative to use.

## Features
* Variables that can hold values
* Arbitrary precision numbers allow you to calculate very large numbers, some high limits are put in for performance reasons
* Autosaving that quickly lets you go back to where you were
* A large library of math functions
* Bitwise operations (keywords are `bxor`, `bor` and `band` to not clash with some other math notations)
* Hex and binary numbers (`0xE32C2C`, `0b10`)

## Installation
1. Download a zip file from the [Releases](https://github.com/AntonBergaker/DoodleDigits/releases)
   - The one marked *Runtime Dependent* requires .NET5 to be installed on your computer. Running the .exe will instruct you how to get it.
   - The one marked *Standalone* does not require any additional downloads.

2. Unzip the file into an empty directory

3. Run DoodleDigits.exe to start the calculator

## Contributing
The project uses NET5.0 and C# 9 as I like having all the latest features.  
As in most projects, make sure whatever you submit is unit tested if necessary and that all tests pass.  
Commits with emoji are preferred.
