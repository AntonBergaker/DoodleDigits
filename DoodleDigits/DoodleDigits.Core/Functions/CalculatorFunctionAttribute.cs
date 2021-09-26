using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Functions {

    [AttributeUsage(AttributeTargets.Method)]
    class CalculatorFunctionAttribute : Attribute {

        public CalculatorFunctionAttribute(string name, params string[] aliases) {

        }

        public CalculatorFunctionAttribute(int argumentCount, string name, params string[] aliases) {

        }

        public CalculatorFunctionAttribute(int argumentCountMin, int argumentCountMax, string name, params string[] aliases) {

        }
    }
}
